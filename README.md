# Amazon-eCommerce

Hello Guys, I this is an eCommerce Application resembling the real Amazon website. I used Angular for the Front End, .NET Core for the Backend and SQL Server for the database for this project. As of now, this is not a fully working project and exactly not all the features are included right now, but this eCommerce application will look as close to the real appliation as time goes on.

To access the Backend for this project navigate to the master section to find the code. For the Frontend, go to the main branch. When downloading the ZIP File when pressing the green code button, make sure to choose the 'main' branch first to download the ZIP file for the Frontend which uses Angular for the project. To download the Backend project which uses .NET CORE, make sure to click on the Master branch before you press the green code button to download the Zip file.




# Backend

For the Backend Section of this project, open the project and run the solution or project file. Extract the content of the master zip folder to a new folder on the desktop. Then you can either click on the solution file (Amazon-eCommerce API.sln) or the project file (Amazon-eCommerce API.csproj). I recommend using Visual Studio or VS Code to open  this projects.

In the solution Explorer you can click on the arrow to drop down and look at the contents. Which shows Controllers, Models, Repositories, Data and more folders.

The backend is mainly used to test and do CRUD operations for products, users and more items for Amazon. I have integrated SwaggerUI in this backend project to test operations. to Use Swagger go to the Green Arrow at the top and to the right click on the dropdown arrow and select IIS Express. This will Utilize SwaggerUI when you press the Green run button. There are CRUD Operations for Products, ProductTypes, ProductBrands  and many more.

# Debugging

Debugging the backend Project is very crucial because many times, your projects might encounter an error and debugging can help solve your problem. SwaggerUI calls can fail, and to find the problem you can debug them.
To debug the SwaggerUI calls. Simply go to the controller you are finding items for (ex. If doing CRUD operations on Products, go to the Products Controller.) To the left side, add a red dot next to the repositories. Also on the call you are doing (ex. GetProducts ) you can put the red dot at the starting Curly bracket and at the return statement. 

![Amazon eCommerce API - How to Debug](https://github.com/your-username/your-repo-name/blob/main/Assets/README-images/Amazon-eCommerce%20API%20-%20How%20to%20Debug.png)







Next when running Swagger UI and pressing the execute button to run the call. A yellow arrow will appear at the left. You will keep pressing F11 to debug the code line by line. This way we can understand how the products will be fetched from the database and what is going on in each line of the code.




## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The application will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via a platform of your choice. To use this command, you need to first add a package that implements end-to-end testing capabilities.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI Overview and Command Reference](https://angular.io/cli) page.
