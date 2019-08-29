# Storytel WebApi is ASP.NET Core WebApi application with Versioning & Swagger

This repository contain Simple Message application. We have 2 entity on this application. User and Message. 
Only register users can perform actions. So we have simple autentication for detect user that enterd the application.

'swagger'
The Api contain 3 controller for intract with clinet:
1) JWTAuthentication : pass 'UserName' and 'password' for login to the application and in the response generate JWT token for furdure requests. This toke must add in authorization header as Bearer token.
2) User :