using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Moq;
using Newtonsoft.Json;
using System.Net;

public class ApiServiceTests
{
    [Fact]
    public async Task GetEmployeesAsync_ShouldReturnListOfEmployees()
    {
        // Arrange
        var mockHttpClient = new Mock<IHttpClient>();
        mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>()))
                      .ReturnsAsync(new HttpResponseMessage
                      {
                          StatusCode = HttpStatusCode.OK,
                          Content = new StringContent(JsonConvert.SerializeObject(
                          
                            new List<Employee>
                              {
                                   new Employee { Id = 1, Name = "John", Email = "john@example.com", Gender = "Male", Status = "Active" },
                                   new Employee { Id = 2, Name = "Jane", Email = "jane@example.com", Gender = "Female", Status = "Active" }
                              }
                          ))
                      });

        var apiService = new ApiService(mockHttpClient.Object);

        // Act
        var employees = await apiService.GetEmployeesAsync();

        // Assert
        Assert.NotNull(employees);
        Assert.Equal(2, employees.Count);
    }

    [Fact]
    public async Task GetEmployeeAsync_ShouldReturnSingleEmployee()
    {
        // Arrange
        var mockHttpClient = new Mock<IHttpClient>();
        mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>()))
                      .ReturnsAsync(new HttpResponseMessage
                      {
                          StatusCode = HttpStatusCode.OK,
                          Content = new StringContent(JsonConvert.SerializeObject(
                          
                         
                                  new Employee { Id = 1, Name = "John", Email = "john@example.com", Gender = "Male", Status = "Active" }
                              
                          ))
                      });

        var apiService = new ApiService(mockHttpClient.Object);

        // Act
        var employee = await apiService.GetEmployeeAsync(1);

        // Assert
        Assert.NotNull(employee);
        Assert.Equal("John", employee.Name);
    }

    [Fact]
    public async Task GetAllEmployeesAsync_ShouldReturnListOfEmployees()
    {
        // Arrange
        var mockHttpClient = new Mock<IHttpClient>();
        mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>()))
                      .ReturnsAsync(new HttpResponseMessage
                      {
                          StatusCode = HttpStatusCode.OK,
                          Content = new StringContent(JsonConvert.SerializeObject(
                          
                               new List<Employee>
                              {
                              new Employee { Id = 1, Name = "John", Email = "john@example.com", Gender = "Male", Status = "Active" },
                              new Employee { Id = 2, Name = "Jane", Email = "jane@example.com", Gender = "Female", Status = "Active" }
                              }
                          ))
                      });

        var apiService = new ApiService(mockHttpClient.Object);

        // Act
        var employee = await apiService.GetAllEmployeesAsync();

        // Assert
        Assert.NotNull(employee);
        Assert.Equal(2, employee.Count);
    }


    [Fact]
    public async Task CreateEmployeeAsync_ShouldReturnCreatedEmployee()
    {
        // Arrange
        var mockHttpClient = new Mock<IHttpClient>();
        var newEmployee = new Employee { Id = 3, Name = "Alice", Email = "alice@example.com", Gender = "Female", Status = "Active" };
        mockHttpClient.Setup(client => client.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                      .ReturnsAsync(new HttpResponseMessage
                      {
                          StatusCode = HttpStatusCode.OK,
                          Content = new StringContent(JsonConvert.SerializeObject(newEmployee))
                      });

        var apiService = new ApiService(mockHttpClient.Object);

        // Act
        var createdEmployee = await apiService.CreateEmployeeAsync(newEmployee);

        // Assert
        Assert.NotNull(createdEmployee);
        Assert.Equal(3, createdEmployee.Id);
        Assert.Equal("Alice", createdEmployee.Name);
    }

    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnUpdatedEmployee()
    {
        // Arrange
        var mockHttpClient = new Mock<IHttpClient>();
        var updatedEmployee = new Employee { Id = 1, Name = "Updated John" };
        mockHttpClient.Setup(client => client.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                      .ReturnsAsync(new HttpResponseMessage
                      {
                          StatusCode = HttpStatusCode.OK,
                          Content = new StringContent(JsonConvert.SerializeObject(updatedEmployee))
                      });

        var apiService = new ApiService(mockHttpClient.Object);

        // Act
        var result = await apiService.UpdateEmployeeAsync(1, updatedEmployee);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Updated John", result.Name);
    }

    [Fact]
    public async Task DeleteEmployeeAsync_ShouldReturnTrueOnSuccess()
    {
        // Arrange
        var mockHttpClient = new Mock<IHttpClient>();
        mockHttpClient.Setup(client => client.DeleteAsync(It.IsAny<string>()))
                      .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent });

        var apiService = new ApiService(mockHttpClient.Object);

        // Act
        var result = await apiService.DeleteEmployeeAsync(1);

        // Assert
        Assert.True(result);
    }
}
