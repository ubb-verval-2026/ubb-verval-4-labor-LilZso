using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace DatesAndStuff.Web.Tests;

[TestFixture]
public class BlazeDemoTests
{
    private IWebDriver driver;

    [SetUp]
    public void Setup()
    {
        driver = new ChromeDriver();
    }

    [TearDown]
    public void Teardown()
    {
        try
        {
            driver?.Quit();
            driver?.Dispose();
        }
        catch (Exception)
        {
            // Ignore errors if unable to close the browser
        }
    }

    [Test]
    public void FindFlights_FromMexicoCityToDublin_ShouldHaveAtLeastThreeFlights()
    {
        // Arrange
        driver.Navigate().GoToUrl("https://blazedemo.com/");

        // Act
        // Select Departure City
        var fromPortSelect = new SelectElement(driver.FindElement(By.Name("fromPort")));
        fromPortSelect.SelectByText("Mexico City");

        // Select Destination City
        var toPortSelect = new SelectElement(driver.FindElement(By.Name("toPort")));
        toPortSelect.SelectByText("Dublin");

        // Click on "Find Flights"
        var findFlightsButton = driver.FindElement(By.CssSelector("input[type='submit']"));
        findFlightsButton.Click();

        // Wait for the results table to load
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
        wait.Until(d => d.FindElements(By.CssSelector("table.table tbody tr")).Count > 0);

        // Assert
        var flightRows = driver.FindElements(By.CssSelector("table.table tbody tr"));
        
        flightRows.Count.Should().BeGreaterThanOrEqualTo(3, "because there should be at least three flights between Mexico City and Dublin");
    }
}
