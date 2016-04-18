# Aaron's Visual Studio Templates
## 0. License

All MIT unless noted.

## 1. Language-less

### 1.1. Copy-Content Template

This project will simply copy all files in the project with Build Action = Content to the output directory when building.

Useful for simple static HTML/CSS/JS stuff when you don't need anything beyond the files themselves.

Based on the [excellent article by Eilon Lipton](http://weblogs.asp.net/leftslipper/creating-visual-studio-projects-that-only-contain-static-files).

### 1.2. Zip-Content Template

This project will simply add all files in the project with Build Action = Content to a zip in the output directory when building.

Useful for creating simple zip packages, or for messing with the guts of Office documents.

Uses [msbuildtasks](https://github.com/loresoft/msbuildtasks) via Nuget.
