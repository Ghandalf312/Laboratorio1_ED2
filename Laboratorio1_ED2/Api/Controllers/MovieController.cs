using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Text.Json;
using ClassLibrary;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        public MovieController(IWebHostEnvironment env)
        {
            Singleton.Instance.path = env.ContentRootPath + "//testapi.txt";
        }

        [HttpGet]
        public IEnumerable<Movie> Get()
        {
            if (Singleton.Instance.Tree != null)
                return Singleton.Instance.Tree.Inorden();
            else
                return null;
        }

        [HttpGet]
        [Route("{traversal}")]
        public IEnumerable<Movie> Get(string travesal)
        {
            if (Singleton.Instance.Tree == null)
                return null;
            else if (travesal == "preorden")
                return Singleton.Instance.Tree.Preorden();
            else if (travesal == "inorden")
                return Singleton.Instance.Tree.Inorden();
            else if (travesal == "postorden")
                return Singleton.Instance.Tree.Postorden();
            else
                return null;
        }

        [HttpPost]
        public IActionResult Create([FromBody] int order)
        {
            try
            {
                Movie testmovie = new Movie();
                Singleton.Instance.Tree = new BTree<Movie>(Singleton.Instance.path, order, testmovie.ToFixedString().Length);
                return StatusCode(201);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        [HttpDelete]
        public IActionResult Clear()
        {
            if (Singleton.Instance.Tree != null)
            {
                Singleton.Instance.Tree.Clear();
            }
            return Ok();
        }


        [HttpPost]
        [Route("populate")]
        public IActionResult Add([FromBody] List<Movie> list)
        {
            try
            {
                if (Singleton.Instance.Tree != null)
                {
                    foreach (var item in list)
                    {
                        item.SetID();
                        Singleton.Instance.Tree.Add(item);
                    }
                    return Ok();
                }
                else
                    return StatusCode(500);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    

        [HttpDelete]
        [Route("populate/{id}")]
        public IActionResult Delete(string id)
        {
            try
            {
                if (Singleton.Instance.Tree != null)
                {
                    var item = new Movie();
                    item.SetID(id);
                    if (Singleton.Instance.Tree.Delete(item))
                        return Ok();
                    else
                        return NotFound();
                }
                else
                    return StatusCode(500);
            }
            catch
            {
                return StatusCode(500);
            }
        }


    }
}
