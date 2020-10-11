using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using TeleworldShop.Data.Infrastructure;
using TeleworldShop.Data.Repositories;
using TeleworldShop.Model.Models;
using TeleworldShop.Service;

namespace TeleworldShop.UnitTest.ServiceTest
{
    [TestClass]
    public class PostCategoryServiceTest
    {
        private Mock<IPostCategoryRepository> mockRepository;
        private Mock<IUnitOfWork> mockUnitOfWork;
        private PostCategoryService postCategoryService;
        private List<PostCategory> postCategories;

        [TestInitialize]
        public void Initialize()
        {
            mockRepository = new Mock<IPostCategoryRepository>();
            mockUnitOfWork = new Mock<IUnitOfWork>();
            postCategoryService = new PostCategoryService(mockRepository.Object, mockUnitOfWork.Object);
            postCategories = new List<PostCategory>()
            {
                new PostCategory() { Id=1,Name="DM1",Status=true},
                new PostCategory() { Id = 2, Name = "DM2", Status = true },
                new PostCategory() { Id = 3, Name = "DM3", Status = true },
            };
        }

        [TestMethod]
        public void PostCategory_Service_GetAll()
        {
            //Set up method
            mockRepository.Setup(m => m.GetAll(null)).Returns(postCategories);
            //Call action
            var result = postCategoryService.GetAll() as List<PostCategory>;
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void PostCategory_Service_Create()
        {
            PostCategory postCategory = new PostCategory();
            postCategory.Name = "Test";
            postCategory.Alias = "TEST";
            postCategory.Status = true;
            mockRepository.Setup(m => m.Add(postCategory)).Returns((PostCategory p) =>
              {
                  p.Id = 1;
                  return p;
              });
            var result = postCategoryService.Add(postCategory);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
        }
    }
}