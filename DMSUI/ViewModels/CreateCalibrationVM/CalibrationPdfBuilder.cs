using System;
using System.Linq;
using DMSUI.Entities.DTOs.Calibration;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DMSUI.ViewModels.Calibration
{
	public static class CalibrationPdfBuilder
	{
		public static byte[] Build(CalibrationItemDTO m, byte[]? logoBytes)
		{
			QuestPDF.Settings.License = LicenseType.Community;

			string Show(string? v) => string.IsNullOrWhiteSpace(v) ? "-" : v.Trim();

			string DueStatusText(int remainingDays)
			{
				if (remainingDays < 0) return "SÜRESİ GEÇTİ";
				if (remainingDays <= 7) return "ACİL";
				if (remainingDays <= 30) return "YAKLAŞIYOR";
				return "UYGUN";
			}

			var theme = new
			{
				PageBg = Colors.Grey.Lighten5,
				Border = Colors.Grey.Lighten2,
				SectionTitleBg = Colors.Blue.Lighten4,
				SectionTitleText = Colors.Blue.Darken3,
				KeyBg = Colors.Grey.Lighten4,
				HeadBg = Colors.Blue.Lighten3,
				HeadText = Colors.Blue.Darken4,
				Zebra = Colors.Grey.Lighten5
			};

			return QuestPDF.Fluent.Document.Create(doc =>
			{
				doc.Page(page =>
				{
					page.Background(theme.PageBg);
					page.Size(PageSizes.A4);
					page.Margin(25);
					page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.Grey.Darken4));

					// Hücre stilleri
					IContainer CellKey(IContainer c) =>
						c.Padding(5)
						 .Background(theme.KeyBg)
						 .Border(1).BorderColor(theme.Border)
						 .AlignMiddle()
						 .DefaultTextStyle(t => t.SemiBold().FontColor(Colors.Grey.Darken3));

					IContainer CellVal(IContainer c) =>
						c.Padding(5)
						 .Background(Colors.White)
						 .Border(1).BorderColor(theme.Border)
						 .AlignMiddle()
						 .DefaultTextStyle(t => t.FontColor(Colors.Grey.Darken4));

					IContainer CellHead(IContainer c) =>
						c.Padding(5)
						 .Background(theme.HeadBg)
						 .Border(1).BorderColor(theme.Border)
						 .AlignMiddle()
						 .DefaultTextStyle(t => t.SemiBold().FontColor(theme.HeadText));

					page.Header().Column(h =>
					{
						h.Item().Row(row =>
						{
							row.ConstantItem(70).Height(40).AlignMiddle().AlignLeft().Element(e =>
							{
								if (logoBytes != null)
									e.Image(logoBytes);
								else
									e.Text("LOGO").FontSize(14).SemiBold();
							});

							row.RelativeItem().AlignMiddle().Column(col =>
							{
								col.Item().Text(Show(m.CompanyName)).FontSize(14).SemiBold();
								col.Item().Text("KALİBRASYON FORMU").FontSize(12).SemiBold().FontColor(theme.SectionTitleText);
								col.Item().Text($"Sertifika No: {Show(m.CertificateNo)}").FontSize(10);
							});

							row.ConstantItem(190).AlignMiddle().Column(col =>
							{
								col.Item().Text($"Kalibrasyon Tarihi: {m.CalibrationDate:dd.MM.yyyy}").AlignRight();
								col.Item().Text($"Sonraki Tarih: {m.DueDate:dd.MM.yyyy}").AlignRight();
								col.Item().Text($"Durum: {DueStatusText(m.RemainingDays)} ({m.RemainingDays} gün)").AlignRight();
							});
						});

						h.Item().PaddingTop(8).LineHorizontal(1).LineColor(theme.Border);
					});

					page.Content().Column(c =>
					{
						c.Spacing(10);

						void Section(string title, Action<IContainer> content)
						{
							c.Item()
							 .Border(1).BorderColor(theme.Border)
							 .Background(Colors.White)
							 .Column(col =>
							 {
								 col.Item()
									.Background(theme.SectionTitleBg)
									.PaddingVertical(6)
									.PaddingHorizontal(8)
									.Text(title)
									.FontSize(11)
									.SemiBold()
									.FontColor(theme.SectionTitleText);

								 col.Item().Padding(8).Element(content);
							 });
						}

						Section("GENEL BİLGİLER", body =>
						{
							body.Table(t =>
							{
								t.ColumnsDefinition(cols =>
								{
									cols.RelativeColumn(1);
									cols.RelativeColumn(2);
									cols.RelativeColumn(1);
									cols.RelativeColumn(2);
								});

								void Row(string k1, string v1, string k2, string v2)
								{
									t.Cell().Element(CellKey).Text(k1);
									t.Cell().Element(CellVal).Text(v1);
									t.Cell().Element(CellKey).Text(k2);
									t.Cell().Element(CellVal).Text(v2);
								}

								Row("Kalibrasyon ID", m.CalibrationId.ToString(), "Cihaz ID", m.InstrumentId.ToString());
								Row("Firma", Show(m.CompanyName), "Kalibrasyon Firması", Show(m.CalibrationCompany));
								Row("Sonuç", Show(m.Result), "Periyot (Ay)", m.IntervalMonths.ToString());
							});
						});

						Section("CİHAZ BİLGİLERİ", body =>
						{
							body.Table(t =>
							{
								t.ColumnsDefinition(cols =>
								{
									cols.RelativeColumn(1);
									cols.RelativeColumn(2);
									cols.RelativeColumn(1);
									cols.RelativeColumn(2);
								});

								void Row(string k1, string v1, string k2, string v2)
								{
									t.Cell().Element(CellKey).Text(k1);
									t.Cell().Element(CellVal).Text(v1);
									t.Cell().Element(CellKey).Text(k2);
									t.Cell().Element(CellVal).Text(v2);
								}

								Row("Demirbaş Kodu", Show(m.AssetCode), "Cihaz Adı", Show(m.InstrumentName));
								Row("Seri No", Show(m.SerialNo), "Lokasyon", Show(m.InstrumentLocation));
							});
						});

						Section("TARİHLER", body =>
						{
							body.Table(t =>
							{
								t.ColumnsDefinition(cols =>
								{
									cols.RelativeColumn(1);
									cols.RelativeColumn(2);
									cols.RelativeColumn(1);
									cols.RelativeColumn(2);
								});

								void Row(string k1, string v1, string k2, string v2)
								{
									t.Cell().Element(CellKey).Text(k1);
									t.Cell().Element(CellVal).Text(v1);
									t.Cell().Element(CellKey).Text(k2);
									t.Cell().Element(CellVal).Text(v2);
								}

								Row("Kalibrasyon Tarihi", m.CalibrationDate.ToString("dd.MM.yyyy"),
									"Sonraki Kalibrasyon", m.DueDate.ToString("dd.MM.yyyy"));

								Row("Kalan Gün", m.RemainingDays.ToString(),
									"Durum", DueStatusText(m.RemainingDays));
							});
						});

						// NOTLAR
						Section("NOTLAR", body =>
						{
							body
								.Border(1).BorderColor(theme.Border)
								.Background(Colors.White)
								.Padding(8)
								.Text(Show(m.Notes));
						});


						// İMZA / KAYIT
						c.Item()
						 .PaddingTop(5)
						 .Border(1).BorderColor(theme.Border)
						 .Background(Colors.White)
						 .Padding(8)
						 .Table(t =>
						 {
							 t.ColumnsDefinition(cols =>
							 {
								 cols.RelativeColumn(1);
								 cols.RelativeColumn(1);
							 });

							 void Box(string title, string? fullName, DateTime? date)
							 {
								 t.Cell()
								  .Border(1).BorderColor(theme.Border)
								  .Background(Colors.White)
								  .Padding(8)
								  .Column(col =>
								  {
									  col.Item()
										 .Background(theme.SectionTitleBg)
										 .Padding(4)
										 .Text(title)
										 .SemiBold()
										 .FontColor(theme.SectionTitleText);

									  col.Item().PaddingTop(18).Text($"Ad Soyad: {Show(fullName)}");
									  col.Item().PaddingTop(10).Text("İmza: ________________________");
									  col.Item().PaddingTop(10).Text($"Tarih: {(date.HasValue ? date.Value.ToString("dd.MM.yyyy") : "-")}");
								  });
							 }

							 Box("Oluşturan", m.CreatedByName, m.CreatedAt);
							 Box("Güncelleyen", m.UpdatedByName, m.UpdatedAt);
						 });
					});

					// Footer
					page.Footer().AlignCenter().Text(x =>
					{
						x.Span("Sayfa ");
						x.CurrentPageNumber();
						x.Span(" / ");
						x.TotalPages();
					});
				});
			}).GeneratePdf();
		}
	}
}
