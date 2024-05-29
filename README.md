Name: Marie Keldrima
Uni-ID: makeld
Student ID: 223191 IADB

----------------------------------------------------------------------------------------


# MVC WEB
~~~bash
dotnet tool update -g dotnet-ef
dotnet tool update -g dotnet-aspnet-codegenerator

dotnet ef migrations --project App.DAL.EF --startup-project WebApp add Initial
dotnet ef database --project App.DAL.EF --startup-project WebApp update

dotnet aspnet-codegenerator controller -name TripsController -actions -m Domain.Entities.Trip -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name CategoriesController -actions -m Domain.Entities.Category -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name CommentsController -actions -m Domain.Entities.Comment -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name CountriesController -actions -m Domain.Entities.Country -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name ExpensesController -actions -m Domain.Entities.Expense -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name LikesController -actions -m Domain.Entities.Like -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name LocationsController -actions -m Domain.Entities.Location -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name PhotosController -actions -m Domain.Entities.Photo -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name TripCategoriesController -actions -m Domain.Entities.TripCategory -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name TripLocationsController -actions -m Domain.Entities.TripLocation -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name TripUsersController -actions -m Domain.Entities.TripUser -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
dotnet aspnet-codegenerator controller -name UserExpensesController -actions -m Domain.Entities.UserExpense -dc AppDbContext -outDir Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
~~~

# API
~~~bash
dotnet aspnet-codegenerator controller -name TripsController  -m  Domain.Entities.Trip       -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name CategoriesController  -m  Domain.Entities.Category       -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name CommentsController  -m  Domain.Entities.Comment       -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name CountriesController  -m  Domain.Entities.Country         -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name ExpensesController  -m  Domain.Entities.Expense       -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name LikesController  -m  Domain.Entities.Like       -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name LocationsController  -m  Domain.Entities.Location       -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name PhotosController  -m  Domain.Entities.Photo       -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name TripCategoriesController  -m  Domain.Entities.TripCategory       -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name TripLocationsController  -m  Domain.Entities.TripLocation       -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name TripUsersController  -m  Domain.Entities.TripUser        -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
dotnet aspnet-codegenerator controller -name UserExpensesController  -m  Domain.Entities.UserExpense        -dc AppDbContext -outDir ApiControllers -api --useAsyncActions -f
~~~