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
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TagManageController : Controller
    {
        private ITagService _tagService;
        private ILogger _logger;

        public TagManageController(ITagService tagService, ILogger<TagManageController> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            TagManageIndexViewModel viewModel = new TagManageIndexViewModel();
            viewModel.Tags = await _tagService.ListAsync();
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

                var Tag = _tagService.GetByIdAsync((int)id);

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

                _tagService.DeleteAsync((int)id);
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
                    _tagService.SaveAsync(tag);

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

        /// <summary>
        /// 创建标签，webapi
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateTag(Tag tag)
        {
            try
            {
                if(tag.Id > 0)
                {
                    return Json(ResultModel.Fail("此标签已创建"));
                }
                else
                {
                    tag.CreateTime = DateTime.Now;
                    tag.LastEditTime = DateTime.Now;
                    _tagService.SaveAsync(tag);
                    return Json(ResultModel.Success(tag, "创建成功"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新建标签失败");
                return Json(ResultModel.Fail(ex, "新建标签失败"));
            }
        }
    }
}
