# LOS-API

Simple REST API written in .net

## Requirement from test file

1. Create a web api project name los-api
2. Create CRUD api for 2 tables (Using in memory table)
   - Product table with following fields Id,Name,ImageUrl,Price
   - Stock table with following fields Id,ProductId,Amount
3. Create a get stock api by specified productId as a parameter and get product detail and stock available as a result

## Usage

```
dotnet run
```

You can use swagger UI to interact with API from this url

[swagger](https://localhost:5001/swagger/index.html)

Feel free to change the port to whatever you want.

## License

[MIT](https://choosealicense.com/licenses/mit/)
