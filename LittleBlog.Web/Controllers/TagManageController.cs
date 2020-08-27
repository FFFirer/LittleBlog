using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LittleBlog.Web.Models;
using LittleBlog.Web.Models.ViewModels.TagManage;
using LittleBlog.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace LittleBlog.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TagManageController : Controller
    {
        private ITagService _tagService;
        private ILogger _logger;

        public TagManageController(ITagService tagService, ILogger<TagManageController> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            TagManageIndexViewModel viewModel = new TagManageIndexViewModel();
            viewModel.Tags = _tagService.Get();
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

                var Tag = _tagService.GetById((int)id);

                if (Tag == null)
                {
                    return NotFound();
                }

                return View(Tag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"找不到要编辑的tag,id:{id}");
                return Forbid();
            }
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            try
            {
                if(id == null)
                {
                    return NotFound();
                }

                _tagService.Delete((int)id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(nameof(Tag), "删除标签失败！");
                _logger.LogError(ex, $"删除tag失败, id:{id}");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(tag);
                }
                else
                {
                    _tagService.Save(tag);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存tag失败");
                ModelState.AddModelError(nameof(Tag), "保存出错！");
                return View(tag);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("Edit", new Tag());
        }
    }
}
