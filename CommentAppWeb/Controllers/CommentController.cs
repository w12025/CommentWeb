using System.Net.Http.Headers;
using System.Text;
using CommentAppWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CommentAppWeb.Controllers;

public class CommentController : Controller
{
    string BASE_URL = "http://ec2-44-200-43-91.compute-1.amazonaws.com/";

    public async Task<ActionResult> Index()
    {
        List<Comment>? comments = new List<Comment>();
        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Clear();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await client.GetAsync("api/v1/comments");

            if (responseMessage.IsSuccessStatusCode)
            {
                var prResponse = responseMessage.Content.ReadAsStringAsync().Result;

                comments = JsonConvert.DeserializeObject<List<Comment>>(prResponse);
            }
        }

        return View(comments);
    }

    public async Task<ActionResult> Details(int id)
    {
        Comment comment = null;

        using (var client = new HttpClient())
        {
            client.BaseAddress = new Uri(BASE_URL);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage responseMessage = await client.GetAsync($"api/v1/comments/{id}");

            if (responseMessage.IsSuccessStatusCode)
            {
                var prResponse = responseMessage.Content.ReadAsStringAsync().Result;
                comment = JsonConvert.DeserializeObject<Comment>(prResponse);
            }
        }

        return View(comment);
    }

    public async Task<ActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create(Comment viewModel)
    {
        var comment = new Comment
        {
            Id = viewModel.Id,
            Title = viewModel.Title,
            Description = viewModel.Description,
            Username = viewModel.Username
        };

        using var client = new HttpClient();
        client.BaseAddress = new Uri(BASE_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var customerJson = JsonConvert.SerializeObject(comment);
        var content = new StringContent(customerJson, Encoding.UTF8, "application/json");

        var response = await client.PostAsync("api/v1/comments", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index"); // Redirect to the Comment list
        }
        else
        {
            return View("Error");
        }
    }

    public async Task<ActionResult> Edit(int id)
    {
        var comment = new Comment();

        using var client = new HttpClient();
        client.BaseAddress = new Uri(BASE_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.GetAsync($"api/v1/comments/{id}");
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            comment = JsonConvert.DeserializeObject<Comment>(responseContent);
        }

        return View(comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit(int id, Comment viewModel)
    {
        var comment = new Comment
        {
            Id = viewModel.Id,
            Title = viewModel.Title,
            Description = viewModel.Description,
            Username = viewModel.Username
        };

        using var client = new HttpClient();
        client.BaseAddress = new Uri(BASE_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var customerJson = JsonConvert.SerializeObject(comment);
        var content = new StringContent(customerJson, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"api/v1/comments", content);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }

        return View(comment);
    }

    public async Task<ActionResult> DeleteAsync(int id)
    {
        var comment = new Comment();

        using var client = new HttpClient();
        client.BaseAddress = new Uri(BASE_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.GetAsync($"api/v1/comments/{id}");
        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            comment = JsonConvert.DeserializeObject<Comment>(responseContent);
        }

        return View(comment);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id, IFormCollection collection)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(BASE_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.DeleteAsync($"api/v1/comments/{id}");

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index"); // Redirect to the order list or another appropriate action
        }
        else
        {
            return View("Error");
        }
    }
}
