Name: Marie Keldrima
Uni-ID: makeld
Student ID: 223191 IADB

----------------------------------------------------------------------------------------

EF
~bash
dotnet tool update --global dotnet-ef

dotnet ef migrations add --project DAL --startup-project WebApp InitialCreate
~

Webpage
~bash
dotnet tool install --global dotnet-aspnet-codegenerator

cd WebApp

dotnet aspnet-codegenerator razorpage \
-m Domain.Answer \
-dc AppDbContext \
-udl \
-outDir Pages/Answers  \
--referenceScriptLibraries \

dotnet aspnet-codegenerator razorpage \
-m Domain.Question \
-dc AppDbContext \
-udl \
-outDir Pages/Questions  \
--referenceScriptLibraries \

dotnet aspnet-codegenerator razorpage \
-m Domain.Quiz \
-dc AppDbContext \
-udl \
-outDir Pages/Quizzes  \
--referenceScriptLibraries \

dotnet aspnet-codegenerator razorpage \
-m Domain.QuizQuestion \
-dc AppDbContext \
-udl \
-outDir Pages/QuizQuestions  \
--referenceScriptLibraries \
