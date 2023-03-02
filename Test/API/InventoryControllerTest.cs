using FluentAssertions;
using InventoryWebService.Controllers;
using InventoryWebService.Models;
using InventoryWebService.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Test.Infrastructure;

namespace Test.API
{
    public class InventoryControllerTest
    {
        private readonly Mock<IInventoryRepository> _repo;

        public InventoryControllerTest()
        {

            _repo = new Mock<IInventoryRepository>();
        }

        [Fact]
        public async Task GetAllAsync()
        {
            //Arrange

            _repo.Setup(x => x.Get(null, null)).ReturnsAsync(MockData.GetMockData());

            var api = new InventoryController(_repo.Object);

            //Act
            var result = await api.Get();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByItemAsync()
        {
            //Arrange
            _repo.Setup(x => x.GetByItem("Apples")).ReturnsAsync(new Inventory
            {
                Name = "Apples",
                Quantity = 10,
                CreatedOn = DateTime.Parse("2000-2-12"),
                LastUpdatedOn = DateTime.Now
            });

            var api = new InventoryController(_repo.Object);

            //Act
            Inventory result = (Inventory)await api.GetById("Apples");

            //Assert
            Assert.Equal(10, result.Quantity);
            Assert.Equal("Apples", result.Name);
        }

        [Fact]
        public async Task GetByItemAsync_NotFound()
        {
            //Arrange
            _repo.Setup(x => x.GetByItem(""));

            var api = new InventoryController(_repo.Object);

            //Act
            var result = await api.GetById("");

            //Assert
            Assert.IsType<NotFoundResult>(result);

        }
        [Fact]
        public async Task CreateAsync()
        {
            var input = new Mock<IEnumerable<Inventory>>();
            //Arrange
            _repo.Setup(x => x.CreateUpdate(input.Object)).ReturnsAsync(true);

            var api = new InventoryController(_repo.Object);

            //Act
            var result = await api.Add(input.Object);
            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task UpdateAsync()
        {
            var input = new Inventory
                    {
                        Name = "Apples",
                       Quantity = 12,
                       CreatedOn = DateTime.Parse("2000-2-12"),
                       LastUpdatedOn = DateTime.Now
                    } ;
            var list = new List<Inventory>();
            list.Add(input);
            //Arrange
            _repo.Setup(x => x.CreateUpdate(list)).ReturnsAsync(true);

            var api = new InventoryController(_repo.Object);

            //Act
            var result = await api.Update("Apples", input);
            var resultList = (List<Inventory>)result;
            //Assert            
            Assert.Equal(12, resultList[0].Quantity);
            Assert.Equal("Apples", resultList[0].Name);
        }
    }
}
