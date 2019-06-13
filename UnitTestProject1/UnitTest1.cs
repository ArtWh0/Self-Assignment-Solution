using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using TechRent.Domain.Abstract;
using TechRent.Domain.Entities;
using TechRent.WebUI.Controllers;
using TechRent.WebUI.Models;
using TechRent.WebUI.Ht_Helping;

namespace GameStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_in_Pages()
        {
            // Организация (arrange)
            Mock<ITechRepository> mock = new Mock<ITechRepository>();
            mock.Setup(m => m.Teches).Returns(new List<Tech>
    {
        new Tech { TechID = 1, Name = "Tech1"},
        new Tech { TechID = 2, Name = "Tech2"},
        new Tech { TechID = 3, Name = "Tech3"},
        new Tech { TechID = 4, Name = "Tech4"},
        new Tech { TechID = 5, Name = "Tech5"}
    });
            TechController controller = new TechController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            TechListViewModel result = (TechListViewModel)controller.List(null, 2).Model;

            // Утверждение
            List<Tech> ta = result.Teches.ToList();
            Assert.IsTrue(ta.Count == 2);
            Assert.AreEqual(ta[0].Name, "Tech4");
            Assert.AreEqual(ta[1].Name, "Tech5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            Paging_Info pagingInfo = new Paging_Info
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<ITechRepository> mock = new Mock<ITechRepository>();
            mock.Setup(m => m.Teches).Returns(new List<Tech>
    {
        new Tech { TechID = 1, Name = "Tech1"},
        new Tech { TechID = 2, Name = "Tech2"},
        new Tech { TechID = 3, Name = "Tech3"},
        new Tech { TechID = 4, Name = "Tech4"},
        new Tech { TechID = 5, Name = "Tech5"}
    });
            TechController controller = new TechController(mock.Object);
            controller.pageSize = 3;

            // Act
            TechListViewModel result
                = (TechListViewModel)controller.List(null, 2).Model;

            // Assert
            Paging_Info pageInfo = result.Paging_Info;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Games()
        {
            // Организация (arrange)
            Mock<ITechRepository> mock = new Mock<ITechRepository>();
            mock.Setup(m => m.Teches).Returns(new List<Tech>
    {

                new Tech { TechID = 1, Name = "Tech1", Category = "Cat1"},
                new Tech { TechID = 2, Name = "Tech2", Category = "Cat2" },
                new Tech { TechID = 3, Name = "Tech3", Category = "Cat1" },
                new Tech { TechID = 4, Name = "Tech4", Category = "Cat2" },
                new Tech { TechID = 5, Name = "Tech5", Category = "Cat3" }
            });
            TechController controller = new TechController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Tech> result = ((TechListViewModel)controller.List("Cat2", 1).Model)
                .Teches.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Tech2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Tech4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<ITechRepository> mock = new Mock<ITechRepository>();
            mock.Setup(m => m.Teches).Returns(new List<Tech> {

                new Tech { TechID = 1, Name = "Tech1", Category = "Cat1"},
                new Tech { TechID = 2, Name = "Tech2", Category = "Cat2" },
                new Tech { TechID = 3, Name = "Tech3", Category = "Cat1" },
                new Tech { TechID = 4, Name = "Tech4", Category = "Cat2" },
                new Tech { TechID = 5, Name = "Tech5", Category = "Cat3" }
    });

            // Организация - создание контроллера
            NavigateController target = new NavigateController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Cat1");
            Assert.AreEqual(results[1], "Cat2");
            Assert.AreEqual(results[2], "Cat3");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<ITechRepository> mock = new Mock<ITechRepository>();
            mock.Setup(m => m.Teches).Returns(new Tech[] {
        new Tech { TechID = 1, Name = "Tech1", Category="Cat1"},
        new Tech { TechID = 2, Name = "Tech2", Category="Cat2"}
    });

            // Организация - создание контроллера
            NavigateController target = new NavigateController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Cat1";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Tech_Count()
        {
            /// Организация (arrange)
            Mock<ITechRepository> mock = new Mock<ITechRepository>();
            mock.Setup(m => m.Teches).Returns(new List<Tech>
    {
                new Tech { TechID = 1, Name = "Tech1", Category = "Cat1"},
                new Tech { TechID = 2, Name = "Tech2", Category = "Cat2" },
                new Tech { TechID = 3, Name = "Tech3", Category = "Cat1" },
                new Tech { TechID = 4, Name = "Tech4", Category = "Cat2" },
                new Tech { TechID = 5, Name = "Tech5", Category = "Cat3" }
    });
            TechController controller = new TechController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((TechListViewModel)controller.List("Cat1").Model).Paging_Info.TotalItems;
            int res2 = ((TechListViewModel)controller.List("Cat2").Model).Paging_Info.TotalItems;
            int res3 = ((TechListViewModel)controller.List("Cat3").Model).Paging_Info.TotalItems;
            int resAll = ((TechListViewModel)controller.List(null).Model).Paging_Info.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }

        [TestClass]
        public class CartTests
        {
            [TestMethod]
            public void Can_Add_New_Lines()
            {
                // Организация - создание нескольких тестовых игр
                Tech tech1 = new Tech { TechID = 1, Name = "Tech1" };
                Tech tech2 = new Tech { TechID = 2, Name = "Tech2" };

                // Организация - создание корзины
                Cart cart = new Cart();

                // Действие
                cart.AddItem(tech1, 1);
                cart.AddItem(tech2, 1);
                List<CartLine> results = cart.Lines.ToList();

                // Утверждение
                Assert.AreEqual(results.Count(), 2);
                Assert.AreEqual(results[0].Tech, tech1);
                Assert.AreEqual(results[1].Tech, tech2);
            }

        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых игр
            Tech tech1 = new Tech { TechID = 1, Name = "Tech1" };
            Tech tech2 = new Tech { TechID = 2, Name = "Tech2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(tech1, 1);
            cart.AddItem(tech2, 1);
            cart.AddItem(tech1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Tech.TechID).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Over_Days, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Over_Days, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых игр
            Tech tech1 = new Tech { TechID = 1, Name = "tech1" };
            Tech tech2 = new Tech { TechID = 2, Name = "tech2" };
            Tech tech3 = new Tech { TechID = 3, Name = "tech3" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - добавление нескольких игр в корзину
            cart.AddItem(tech1, 1);
            cart.AddItem(tech2, 4);
            cart.AddItem(tech3, 2);
            cart.AddItem(tech2, 1);

            // Действие
            cart.RemoveLine(tech2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Tech == tech2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых игр
            Tech tech1 = new Tech { TechID = 1, Name = "tech1", Min_Price = 100, Rent_Price=60};
            Tech tech2 = new Tech { TechID = 2, Name = "tech2", Min_Price = 180, Rent_Price=40};

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(tech1, 1);
            cart.AddItem(tech2, 1);
            cart.AddItem(tech1, 5);
            decimal result_price = cart.ComputeTotalValue();
            
            // Утверждение
            Assert.AreEqual(result_price, 680);            
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
            Tech tech1 = new Tech { TechID = 1, Name = "tech1", Min_Price = 100, Rent_Price = 60 };
            Tech tech2 = new Tech { TechID = 2, Name = "tech2", Min_Price = 180, Rent_Price = 40 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(tech1, 1);
            cart.AddItem(tech2, 1);
            cart.AddItem(tech1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            Cart cart = new Cart();

            // Организация - создание деталей о доставке
            ShippingDetails shippingDetails = new ShippingDetails();

            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Tech(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Tech(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

    }
}
