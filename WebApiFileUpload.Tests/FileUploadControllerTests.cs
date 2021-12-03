using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using WebApiFileUpload.Controllers;
using WebApiFileUpload.Models;
using Xunit;

namespace WebApiFileUpload.Tests
{
    public class FileUploadControllerTests
    {
        [Fact]
        public void Upload_PositiveTest()
        {
            //Arrange
            var fileMock = new Mock<IFormFile>();            
            var content = "Richard 3293982";
            var fileName = "Test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var controller = new FileUploadController();
            var file = fileMock.Object;
            bool expectedResponse = true;

            //Act
            var result = controller.Upload(file);            
            ResponseModel actualResponse = JsonConvert.DeserializeObject<ResponseModel>(((ObjectResult)result).Value.ToString());

            //Assert
            Assert.Matches(actualResponse.fileValid.ToString(), expectedResponse.ToString());
        }

        [Fact]
        public void Upload_NegativeTest()
        {
            //Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "XAEA-12 8293982";
            var fileName = "Test.txt";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            var controller = new FileUploadController();
            var file = fileMock.Object;
            bool expectedResponse = false;

            //Act
            var result = controller.Upload(file);
            ResponseModel actualResponse = JsonConvert.DeserializeObject<ResponseModel>(((ObjectResult)result).Value.ToString());

            //Assert
            Assert.Matches(actualResponse.fileValid.ToString(), expectedResponse.ToString());
        }
    }
}
