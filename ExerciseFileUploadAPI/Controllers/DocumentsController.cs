using ExerciseFileUploadAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Collections.Generic;
using SharedProjects;
using ExerciseFileUploadAPI.Repository;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExerciseFileUploadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private IDocumentRepository _documentrepository;
        private ILogger<DocumentsController> _logger;

        public DocumentsController(IDocumentRepository documentRepository, ILogger<DocumentsController> logger)
        {
            this._documentrepository = documentRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.Log(LogLevel.Information, "Started Get Call");
            try
            {
                var documents = _documentrepository.GetList();
                _logger.Log(LogLevel.Information, "End Get Call");
                return Ok(documents);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Information, "End Get Call");
                return BadRequest(ex.Message);
            }
           
        }

        [HttpPost]
        public IActionResult Post(MyDocuments document)
        {
            _logger.Log(LogLevel.Information, "Start Post Call");
            try
            {
                if (document == null)
                {
                    return BadRequest("Document Need");
                }
                if (document.Content == null && string.IsNullOrEmpty(document.Base64stringFile))
                {
                    return BadRequest("Document Content Need");
                }
                if (document.Content == null)
                {
                    document.Content = Convert.FromBase64String(document.Base64stringFile);
                }
                if (string.IsNullOrEmpty(document.Base64stringFile))
                {
                    document.Base64stringFile = Convert.ToBase64String(document.Content);
                }
                var res = _documentrepository.UploadDocument(document);
                if (res)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Error While Uploding Files");
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("PostList")]
        public IActionResult PostList(MultiMyDocuments documents)
        {
            try
            {
                var result = true;
                foreach (var document in documents.Documents)
                {
                    if (document == null)
                    {
                        return BadRequest("Document Need");
                    }
                    if (document.Content == null && string.IsNullOrEmpty(document.Base64stringFile))
                    {
                        return BadRequest("Document Content Need");
                    }
                    if (document.Content == null)
                    {
                        document.Content = Convert.FromBase64String(document.Base64stringFile);
                    }
                    if (string.IsNullOrEmpty(document.Base64stringFile))
                    {
                        document.Base64stringFile = Convert.ToBase64String(document.Content);
                    }
                    result = _documentrepository.UploadDocument(document);
                    if (!result)
                    {
                        return BadRequest($"Error While Uploding Files Named : {document.FileName}");
                    }
                }

                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
