using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Moq;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Reflection.Metadata;

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
                                   new Employee { id = 1, name = "John", email = "john@example.com", gender = "Male", Status = "Active" },
                                   new Employee { id = 2, name = "Jane", email = "jane@example.com", gender = "Female", Status = "Active" }
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
                          
                         
                                  new Employee { id = 1, name = "John", email = "john@example.com", gender = "Male", Status = "Active" }
                              
                          ))
                      });

        var apiService = new ApiService(mockHttpClient.Object);

        // Act
        var employee = await apiService.GetEmployeeAsync(1);

        // Assert
        Assert.NotNull(employee);
        Assert.Equal("John", employee.name);
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
                              new Employee { id = 1, name = "John", email = "john@example.com", gender = "Male", Status = "Active" },
                              new Employee { id = 2, name = "Jane", email = "jane@example.com", gender = "Female", Status = "Active" }
                              }
                          ))
                      });

        var apiService = new ApiService(mockHttpClient.Object);

        // Act
        var employee = await apiService.GetEmployeesAsync();

        // Assert
        Assert.NotNull(employee);
        Assert.Equal(2, employee.Count);
    }


    [Fact]
    public async Task CreateEmployeeAsync_ShouldReturnCreatedEmployee()
    {
        // Arrange
        var mockHttpClient = new Mock<IHttpClient>();
        var newEmployee = new Employee { id = 3, name = "Alice", email = "alice@example.com", gender = "Female", Status = "Active" };
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
        Assert.Equal(3, createdEmployee.id);
        Assert.Equal("Alice", createdEmployee.name);
    }

    [Fact]
    public async Task UpdateEmployeeAsync_ShouldReturnUpdatedEmployee()
    {
        // Arrange
        var mockHttpClient = new Mock<IHttpClient>();
        var updatedEmployee = new Employee { id = 1, name = "Updated John" };
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
        Assert.Equal(1, result.id);
        Assert.Equal("Updated John", result.name);
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

    [Fact]
    public async Task DeleteEmployeeAsync_ShouldReturnFalseOnFailure()
    {
        // Arrange
        var mockHttpClient = new Mock<IHttpClient>();
        mockHttpClient.Setup(client => client.DeleteAsync(It.IsAny<string>()))
                      .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound });

        var apiService = new ApiService(mockHttpClient.Object);

        // Act
        var result = await apiService.DeleteEmployeeAsync(1);

        // Assert
        Assert.False(result);
    }

   [Fact]
    public async Task UpdateEmployeeAsync_ShouldHandleUpdateFailure()
    {
        // Arrange
        var mockHttpClient = new Mock<IHttpClient>();
        var updatedEmployee = new Employee { id = 1, name = "Updated John" };

        // Setup the mock to return an error response when PutAsync is called
        mockHttpClient.Setup(client => client.PutAsync(It.IsAny<string>(), It.IsAny<HttpContent>()))
                      .ReturnsAsync(new HttpResponseMessage
                      {
                          StatusCode = HttpStatusCode.InternalServerError  // Simulate an error response
                      });

        var apiService = new ApiService(mockHttpClient.Object);

        // Act
        var result = await apiService.UpdateEmployeeAsync(1, updatedEmployee);

        // Assert
        Assert.Null(result);  // Assert that the result is null indicating an error
                              
    }

    [Fact]
    public async Task GetEmployeesAsync_ShouldReturnListOfEmployees_WithPage()
    {
        // Arrange
        var mockHttpClient = new Mock<IHttpClient>();
        var apiService = new ApiService(mockHttpClient.Object);

        var expectedEmployees = new List<Employee>
        {
            new Employee { id = 1, name = "John Doe" },
            new Employee { id = 2, name = "Jane Smith" }
        };

        var responseContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(expectedEmployees));
        responseContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) { Content = responseContent };
        mockHttpClient.Setup(client => client.GetAsync(It.IsAny<string>())).ReturnsAsync(mockResponse);

        SearchParameters sp = new SearchParameters();

        // Act
        var result = await apiService.GetEmployeesAsync(sp);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedEmployees.Count, result.Count);

        for (int i = 0; i < expectedEmployees.Count; i++)
        {
            Assert.Equal(expectedEmployees[i].id, result[i].id);
            Assert.Equal(expectedEmployees[i].name, result[i].name);
        }
    }

}
