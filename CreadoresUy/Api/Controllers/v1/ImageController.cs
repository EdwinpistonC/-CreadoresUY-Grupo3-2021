using Application.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Share.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ImageController : BaseApiController
    {
        public ImagePostService _imageService;

        public ImageController(ImagePostService imageService)
        {
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateUser(ImageDto dto)
        {
            return Ok(await _imageService.postImage(dto));
        }
        /*
        [HttpPost]
        public async Task<ActionResult<string>> CreateUser(ImageDto dto )
        {
            ImageDto dto = new ImageDto
            {
                Image = ,
                ImageName = "prueba",
                FolderName = "usuarios"
            };
            ImagePostService imageps = new ImagePostService();
            return Ok( await imageps.postImage(dto));
        }
    */
    }
}
