![Amazon eCommerce API - Platform](https://github.com/Krishjot8/Amazon-eCommerce/blob/main/Assets/README-images/amazon-platform.png)


# Amazon-eCommerce

Hello everyone,

This is an eCommerce application inspired by the Amazon website. The frontend is built using Angular, while the backend is developed with .NET Core. For the database, I am using SQL Server. Currently, the project is a work in progress, and not all features are fully implemented yet. However, as development continues, this application will closely resemble a real-world eCommerce platform.

To access the backend code, navigate to the 'master' branch. For the frontend code, check the 'main' branch. When downloading the project, if you're interested in the Angular frontend, make sure to switch to the 'main' branch before clicking the green code button to download the ZIP file. If you're looking for the .NET Core backend, switch to the 'master' branch before downloading.




# Backend

To run the backend of this project, follow these steps:

1. Download the ZIP file from the 'master' branch and extract its contents to a new folder on your desktop.
   
2. Open the project by either clicking on the solution file (Amazon-eCommerce API.sln) or the project file (Amazon-eCommerce API.csproj). I recommend using Visual Studio or Visual Studio Code for this project.

Once the project is open, you can explore the contents through the Solution Explorer. Expand the folders to view Controllers, Models, Repositories, Data, and more.

The backend is designed to perform CRUD operations (Create, Read, Update, Delete) for products, users, and other entities related to the eCommerce platform. SwaggerUI is integrated into this project for easy API testing. To use Swagger, go to the top toolbar and click the dropdown arrow next to the green "Run" button. Select IIS Express and then click the green "Run" button to launch SwaggerUI.

You can test CRUD operations for entities such as Products, Product Types, Product Brands, and more using SwaggerUI.

# Debugging the Backend

Debugging is crucial when working on the backend, as errors can occur during development. Debugging can help identify and resolve issues effectively. SwaggerUI calls, for instance, may fail, and debugging those calls can provide insight into the problem.

To debug SwaggerUI calls:

1. Navigate to the appropriate controller related to the operation you're testing (e.g., for CRUD operations on products, go to the ProductsController).
2. In the controller, set breakpoints (represented by red dots) to pause the execution of the code. For example:

   
        • Add a breakpoint next to the repository calls.
        • Place additional breakpoints at the starting curly bracket { and the return statement of the method you're debugging (e.g., GetProducts).

    When you run the project in debug mode, execution will pause at these breakpoints, allowing you to inspect the data and track down issues.


![Amazon eCommerce API - Platform](https://github.com/Krishjot8/Amazon-eCommerce/blob/main/Assets/README-images/Amazon-eCommerce%20API%20-%20How%20to%20Debug.png)




# Using SwaggerUI for Debugging

![Amazon eCommerce API - Platform](https://github.com/Krishjot8/Amazon-eCommerce/blob/main/Assets/README-images/Swagger%20UI.png)

                   
After setting up your breakpoints, you can run SwaggerUI and execute your API calls by pressing the Execute button. When you do this, a yellow arrow will appear on the left side of the code, indicating the current line of execution.

To debug your code step by step, keep pressing F11. This allows you to step through the code line by line, helping you understand how products are fetched from the database and what happens at each stage of the execution. This detailed debugging process is valuable for identifying any issues and gaining insights into the application's behavior.

![Amazon eCommerce API - Platform](https://github.com/Krishjot8/Amazon-eCommerce/blob/main/Assets/README-images/amazon-platform.png)

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
