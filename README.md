# InventoryWebService
#### Created a ASP.NET Core MVC app with the basic CRUD and sort funtionality. Apis have been exposed under the routes and port[8081] shared as in question sheet.

## Tools and version Used-
1. .Net 6
2. SQL Server Express 2019 (This has been commented out for testing purpose)
3. InMemory EFCore - version 6 (This will be working, it has been added so we don't require DB setup to test APIS/MVC app)
4. Visual Studio 2022
5. Postman

## Routes- 
1. Get All - GET - https://localhost:{{port}}/api/Inventory
2. GetByItem - GET - https://localhost:{{port}}/api/Inventory/{itemName}
3. CreateOrUpdate - POST - https://localhost:{{port}}/api/Inventory
4. Update - PUT - https://localhost:{{port}}/api/Inventory/{itemName}
5. Delete - DELETE - https://localhost:{{port}}/api/Inventory/{itemName}
6. Sort - GET - https://localhost:{{port}}/api/Inventory/values?sortBy=quantity_desc&returnVal=all
    
    The sortBy and returnVal query strings can have multiple values - 
    
    **sortBy-**
    *  sortBy = quantity_desc - Order by quantity desc
    *  sortBy = quantity - Order by quantity asc
    *  sortBy = Date - Order by  created on desc
    *  sortBy = date_desc - Order by  created on desc
    *  sortBy = name_desc - Order by  name desc
    *  sortBy = name - Order by  name asc

    **returnVal-**
    * returnVal = 1 - Return Top 1 based on the _sortBy_ specified (Highest/Lowest)
    * returnVal = all -Returns all items in the specicifed order

![EditApi](https://user-images.githubusercontent.com/21274195/222508420-067d83c6-1453-435b-8a08-409c0b455f12.png)

This is the _Index_ page of MVC app.

![InventoryIndex](https://user-images.githubusercontent.com/21274195/222508450-6aff08dc-077f-45ec-82d8-49c8e4809dc1.png)

This is an example of Sort APi-

![Sort](https://user-images.githubusercontent.com/21274195/222509930-4f10500c-7726-4e8a-ad37-a7f5795d0659.png)



## Extra Features-
1. Concurrency conflicts - I have handled concurrency conflict using **RowVersion**. If we have opened the same item in two tabs and one user is updating the _quantity_ in one tab, and the other user tries to update in the second tab, it will throw a ModelState Error- stating that value has been modified by the user and it shows the _current value_ under the _quantity_ field.

  ![ConcurrencyConflict](https://user-images.githubusercontent.com/21274195/222507700-93348bc2-3f5c-4b00-b999-71bbfb7bbbd8.png)
 Also throught API Request, we can see the following error- 
 ![ConcurrencyConflictAPI](https://user-images.githubusercontent.com/21274195/222507880-9800ccae-4786-4c41-ad57-adb0ecac3f69.png)

2. Included _LastUpdatedOn_ field so that we can track the update history.
3. Included Unit test (xUnit) for controllers

_Note_:- RowVersion functionality will only work when connected to sql server express.


