using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.ViewModels.CategoryManage;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LittleBlog.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoryManageController : Controller
    {
        private ICategoryService _categoryService;
        private ILogger _logger;

        public CategoryManageController(ICategoryService categoryService, ILogger<CategoryManageController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            CategoryManageIndexViewModel viewModel = new CategoryManageIndexViewModel();
            viewModel.Categories = _categoryService.Get();
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            try
            {
                if(id == null)
                {
                    return NotFound();
                }

                var Category = _categoryService.GetById((int)id);

                if(Category == null)
                {
                    return NotFound();
                }

                return View(Category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"找不到要编辑的Category,id:{id}");
                return Forbid();
            }
        }
        
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(category);
                }
                else
                {
                    _categoryService.Save(category);

                    return RedirectToAction("Index");
                }
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存category失败");
                ModelState.AddModelError(nameof(Category), "保存出错！");
                return View(category);
            }
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                _categoryService.Delete((int)id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"删除category失败,id:{id}");
                ModelState.AddModelError(nameof(Category), "删除分类失败");
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit", new Category());
        }
    }
}
