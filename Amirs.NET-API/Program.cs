using RestSharp;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
RestClient _client = new RestClient("https://amirsnodeapi.azurewebsites.net/");

/*http://localhost:3000*/

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/getbooks", async () =>
{
    RestRequest request = new RestRequest("/getbooks");
    RestResponse<Books[]> response = await _client.ExecuteAsync<Books[]>(request);
    if (response.IsSuccessful)
    {
        return Results.Ok(response.Data);
    }
    else
    {
        return Results.NotFound(response.ErrorMessage);
    }
});


app.MapPost("/addbook", async (Books books) =>
{
    RestRequest request = new RestRequest("/addbook ");
    request.AddJsonBody(books);
    request.Method = Method.Post;
    RestResponse response = await _client.ExecuteAsync<Books>(request);
    if (response.IsSuccessful)
    {
        return Results.Ok();
        
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapDelete("/delete/{bookname}", async(string bookname) =>
{
    RestRequest request = new RestRequest("/delete/{bookname}");
    request.AddUrlSegment("bookname", bookname);
    request.Method = Method.Delete;
    RestResponse response = await _client.ExecuteAsync(request);
    if (response.IsSuccessful)
    {
        return Results.Ok();
    }
    else
    {
        return Results.NotFound();

    }
});

app.Run();



record Books(int id, string name, string author, string releaseDate, string ISBN);