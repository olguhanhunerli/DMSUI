using System;
using System.Linq;
using DMSUI.Entities.DTOs.Complaints;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace DMSUI.ViewModels.Complaint
{
	public static class ComplaintPdfBuilder
	{
		public static byte[] Build(ComplaintItemsDTO m, byte[]? logoBytes)
		{
			QuestPDF.Settings.License = LicenseType.Community;

			string Show(string? v) => string.IsNullOrWhiteSpace(v) ? "-" : v.Trim();

			string SeverityText(int id) => id switch
			{
				1 => "Düşük",
				2 => "Orta",
				3 => "Yüksek",
				4 => "Kritik",
				_ => "-"
			};

			// Simple theme (QuestPDF palette)
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
								col.Item().Text(Show(m.companyName)).FontSize(14).SemiBold();
								col.Item().Text("MÜŞTERİ ŞİKAYET FORMU").FontSize(12).SemiBold().FontColor(theme.SectionTitleText);
								col.Item().Text($"Şikayet No: {Show(m.complaintNo)}").FontSize(10);
							});

							row.ConstantItem(160).AlignMiddle().Column(col =>
							{
								col.Item().Text($"Tarih: {m.reportedAt:dd.MM.yyyy HH:mm}").AlignRight();
								col.Item().Text($"Durum: {Show(m.status)}").AlignRight();
								col.Item().Text($"Önem: {SeverityText(m.severityId)}").AlignRight();
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

								Row("Müşteri", Show(m.customerName), "Tekrar", m.isRepeat ? "Evet" : "Hayır");
								Row("Atanan", Show(m.assignedToName), "CAPA", m.needsCapa ? "Gerekli" : "Yok");
								Row("Oluşturan", Show(m.createdByName), "Ara Aksiyon", m.interimActionRequired == true ? "Gerekli" : "Gerekli Değil");

								if (!string.IsNullOrWhiteSpace(m.interimActionNote))
								{
									t.Cell().Element(CellKey).Text("Ara Aksiyon Notu");
									t.Cell().ColumnSpan(3).Element(CellVal).Text(Show(m.interimActionNote));
								}
							});
						});

						Section("ŞİKAYET DETAYI", body =>
						{
							body.Table(t =>
							{
								t.ColumnsDefinition(cols =>
								{
									cols.RelativeColumn(1);
									cols.RelativeColumn(5);
								});

								t.Cell().Element(CellKey).Text("Başlık");
								t.Cell().Element(CellVal).Text(Show(m.title));

								t.Cell().Element(CellKey).Text("Açıklama");
								t.Cell().Element(CellVal).Text(Show(m.description));
							});
						});

						Section("ÜRÜN / İZLENEBİLİRLİK", body =>
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

								Row("Parça No", Show(m.partNumber), "Revizyon", Show(m.partRevision));
								Row("Lot No", Show(m.lotNumber), "Seri No", Show(m.serialNumber));
								Row("Üretim Tarihi", m.productionDate?.ToString("dd.MM.yyyy") ?? "-", "Üretim Hattı", Show(m.productionLine));
							});
						});

						Section("MÜŞTERİ / SEVKİYAT", body =>
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

								Row("Müşteri Şikayet No", Show(m.customerComplaintNo), "Müşteri PO", Show(m.customerPO));
								Row("İrsaliye / Sevk No", Show(m.deliveryNoteNo), "Etkilenen Adet", m.quantityAffected?.ToString() ?? "-");
							});
						});

						Section("AKSİYONLAR", body =>
						{
							body.Table(t =>
							{
								t.ColumnsDefinition(cols =>
								{
									cols.RelativeColumn(1);
									cols.RelativeColumn(5);
								});

								t.Cell().Element(CellKey).Text("Containment Aksiyonu");
								t.Cell().Element(CellVal).Text(Show(m.containmentAction));
							});
						});

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
								 cols.RelativeColumn(1);
							 });

							 void SignBox(string title, string? fullName, DateTime? date)
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

							 SignBox("Hazırlayan", m.createdByName, m.createdAt);
							 SignBox("Kontrol", m.assignedToName, m.updatedAt);
							 SignBox("Onay", m.closedByName, m.closedAt);
						 });
					});

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
