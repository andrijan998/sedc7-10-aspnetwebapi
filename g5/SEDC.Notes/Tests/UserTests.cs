using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Services;
using Services.Helpers;

namespace Tests
{
    [TestClass]
    public class UserTests
    {
        [TestMethod]
        public void Authenticate_ValidUsernamePassword_ValidToken()
        {
            // Arrange
            IOptions<AppSettings> mockOptions = Options.Create<AppSettings>(new AppSettings()
            {
                Secret = "SECRET FOR TESTING"
            });
            IUserService userService  = new UserService(MockHelper.MockUserRepository(), mockOptions);
            string username = "bob007";
            string password = "123456sedc";

            // Act
            UserModel result = userService.Authenticate(username, password);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Token != string.Empty && result.Token != null);
        }

        [TestMethod]
        public void Authenticate_InvalidUsernamePassword_Null()
        {
            // Arrange
            IOptions<AppSettings> mockOptions = Options.Create<AppSettings>(new AppSettings()
            {
                Secret = "SECRET FOR TESTING"
            });
            IUserService userService = new UserService(new FakeUserRepository(), mockOptions);
            string username = "NonExisting";
            string password = "123456789";

            // Act
            UserModel result = userService.Authenticate(username, password);

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod]
        public void Register_ValidData_RegisteredUser()
        {
            // Arrange
            IOptions<AppSettings> mockOptions = Options.Create<AppSettings>(new AppSettings()
            {
                Secret = "SECRET FOR TESTING"
            });
            IUserService userService = new UserService(new FakeUserRepository(), mockOptions);
            RegisterModel model = new RegisterModel()
            {
                FirstName = "Greg",
                LastName = "Gregsky",
                Password = "123456greg",
                ConfirmPassword = "123456greg",
                Username = "gregsuper"
            };
            // Act
            userService.Register(model);

            // Assert
            UserModel newUser = userService.Authenticate(model.Username, model.Password);
            Assert.AreEqual(model.FirstName, newUser.FirstName);
            Assert.AreEqual(model.LastName, newUser.LastName);
        }
    }
}
