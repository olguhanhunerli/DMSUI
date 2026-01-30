using DMSUI.Entities.DTOs.Customers;
using DMSUI.Services.Interfaces;
using DMSUI.ViewModels.NewFolder;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMSUI.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerManager _customerManager;

        public CustomerController(ICustomerManager customerManager)
        {
            _customerManager = customerManager;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var entity = await _customerManager.GetCustomersPaging(page, pageSize);
            return View(entity);
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var customer = await _customerManager.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            var entity = new CustomersItemDTO
            {
                id = customer.id,
                customerCode = customer.customerCode,
                name = customer.name,
                phone = customer.phone,
                email = customer.email,
                createdAt = customer.createdAt,
                companyId = customer.companyId,
                companyName = customer.companyName,
                deletedByName = customer.deletedByName,
                deleteAt = customer.deleteAt,
                isDelete = customer.isDelete
            };
            return View(customer);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var c = await _customerManager.GetCustomerById(id);
            if (c == null) return NotFound();

            return View(new EditCustomersVM 
            {
                id = c.id,
                customerCode = c.customerCode,
                name = c.name,
                phone = c.phone,
                email = c.email,
                companyName = c.companyName,
                createdAt = (DateTime)c.createdAt,
                isDelete = (bool)c.isDelete,
                deleteAt = (bool)c.isDelete ? c.deleteAt : null,
                deletedByName = c.deletedByName?.ToString()
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditCustomersVM model)
        {
            if (!ModelState.IsValid)
            {
                foreach (var kvp in ModelState)
                {
                    foreach (var err in kvp.Value.Errors)
                        Console.WriteLine($"{kvp.Key}: {err.ErrorMessage}");
                }
                return View(model);
            }

            var dto = new UpdateCustomerDTO
            {
                customerCode = model.customerCode,
                name = model.name,
                phone = model.phone,
                email = model.email
            };
            var ok = await _customerManager.UpdateCustomer(model.id, dto);
            if (ok) return RedirectToAction("Index");

            ModelState.AddModelError("", "Failed to update customer.");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerManager.DeleteCustomer(id);
            if (result)
            {
                return RedirectToAction("Index");
            }
            return BadRequest("Failed to delete customer.");
        }
    }
}
