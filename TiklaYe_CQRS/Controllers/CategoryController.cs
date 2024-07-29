using Microsoft.AspNetCore.Mvc;
using TiklaYe_CQRS.CommandHandlers;
using TiklaYe_CQRS.Commands;
using TiklaYe_CQRS.Models;
using TiklaYe_CQRS.Queries;
using TiklaYe_CQRS.QueryHandlers;

namespace TiklaYe_CQRS.Controllers
{

    public class CategoryController : Controller
    {
        private readonly CreateCategoryCommandHandler _createHandler;
        private readonly UpdateCategoryCommandHandler _updateHandler;
        private readonly DeleteCategoryCommandHandler _deleteHandler;
        private readonly GetCategoryByIdQueryHandler _getByIdHandler;
        private readonly GetCategoriesQueryHandler _getAllHandler;

        public CategoryController(
            CreateCategoryCommandHandler createHandler,
            UpdateCategoryCommandHandler updateHandler,
            DeleteCategoryCommandHandler deleteHandler,
            GetCategoryByIdQueryHandler getByIdHandler,
            GetCategoriesQueryHandler getAllHandler)
        {
            _createHandler = createHandler;
            _updateHandler = updateHandler;
            _deleteHandler = deleteHandler;
            _getByIdHandler = getByIdHandler;
            _getAllHandler = getAllHandler;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _getAllHandler.Handle(new GetCategoriesQuery());
            return View(categories);
        }

        public IActionResult Create()
        {
            var category = new Category
            {
                IsActive = true // Varsayılan olarak aktif olarak ayarla
            };
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var command = new CreateCategoryCommand
                {
                    Name = category.Name,
                    IsActive = category.IsActive,
                    ImageUrlFile = category.ImageUrlFile
                };

                await _createHandler.Handle(command);
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _getByIdHandler.Handle(new GetCategoryByIdQuery { CategoryId = id.Value });
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var command = new UpdateCategoryCommand
                    {
                        CategoryId = category.CategoryId,
                        Name = category.Name,
                        IsActive = category.IsActive,
                        ImageUrlFile = category.ImageUrlFile
                    };

                    await _updateHandler.Handle(command);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View(category);
                }
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var command = new DeleteCategoryCommand { CategoryId = id };
            await _deleteHandler.Handle(command);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> CategoryGet(int id)
        {
            var category = await _getByIdHandler.Handle(new GetCategoryByIdQuery { CategoryId = id });
            if (category == null)
            {
                return NotFound();
            }

            var result = new
            {
                Id = category.CategoryId,
                Name = category.Name
            };

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> CategoriesGet()
        {
            var categories = await _getAllHandler.Handle(new GetCategoriesQuery());
            var result = categories.Select(c => new
            {
                Id = c.CategoryId,
                Name = c.Name
            }).ToList();

            return Json(result);
        }
    }
}