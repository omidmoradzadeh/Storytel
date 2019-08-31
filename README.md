# Storytel WebApi is ASP.NET Core WebApi application with Versioning & Swagger

This repository contains a Simple Message application. We have 2 entity on this application. User and Message. 
Only register users can perform actions. So we have simple authentication for detect user that entered the application.
Because there is not special Bussines on this application, I connect directly Controller to Repository.

## Controllers
The API contains 3 controllers for interact with a clinet:

1) JWTAuthentication : pass 'UserName' and 'password' for login to the application and in the response generate JWT token for future requests. This toke must add in authorization header as Bearer token. Action is:
    Post : For validate user name and password of a user.

2) User : CRUD operation on User Entity. User has IsAdmin property and only admin user's can call this controller. Actions are:

    * Get: Get special user. (http://{serverAddress}:{port}/api/v1/user/1)
    
    * GetAll : Get all users. (http://{serverAddress}:{port}/api/v1/user)
    
    * Post : Add a new user.	(http://{serverAddress}:{port}/api/v1/user)
    
    * Put : Update existed user information. (http://{serverAddress}:{port}/api/v1/user)
    
    * Delete : Delete user. (http://{serverAddress}:{port}/api/v1/user)
    

3) Message : CRUD operation on Message Entity. Actions are:

    * Get: Get special message.  (http://{serverAddress}:{port}/api/v1/message/1)
    
    * GetAll : Get all messages related to login user. (http://{serverAddress}:{port}/api/v1/message)
    
    * Post : Add a new message. (http://{serverAddress}:{port}/api/v1/message)
    
    * Put : Update existed message information. (http://{serverAddress}:{port}/api/v1/message)
    
    * Delete : Delete message. (http://{serverAddress}:{port}/api/v1/message)
    

## Deploy:
Users can publish application in 2 ways


1) IIS 

   - For publishing this API on IIS, at first you need to install .Net Core from https://dotnet.microsoft.com/download .
	
   - Install Internet Information Service from 'Control panel\Program and Features\Turn on window features on or off' and then check 'Internet Information Service'  
	
   - Go to IIS. you can perform this action by press (win + R) on keyboard and enter inetmgr.
	
   - Expand local IIS ( computer name) and right-click on Sites and click 'Add New Website'
	
   - In New Dialog add a name for 'Site Name', also create a new path for save publish files and click ok.
	
   - Go to IIS manager and below the local IIS click on Appliation pool
	
   - Find your new created Application pool and double click on it.
	
   - Select .NET CLR version as 'No Managed Code'. then click ok.
	
   - Publish your application :
	
   - Go to application path.
	    
   - Run this command: dotnet publish -c Release
	    
   - Go to your application path\bin\Release\netcoreapp2.2\publish
	
   - Copy content of this folder to created site on IIS.
	


2) Docker

   - For publish this API on docker, at first depend on your OS, you need to install Docker form website : https://hub.docker.com/?overlay=onboarding
	
   - Go to path of the application. where the 'Dockerfile' file exists.
	
   - Docker build . -t {ImageName}:{TagName}
	
   - You must fill image name and tag name with what ever you want.
	    
   - After the build was finished, you can find your image :
	
   - Docker images
	    
   - You can run the container :

   - Docker run -d -p 80:80 {ImageName}
	    
   - -p 80:80 port fowrard docker image 80 port to your pc. if you want to use a random port, insted of -p 80:80 you can use -p .
	    
   - For check your docker status and see the random port you can use this command: Docker ps -a
 
   - Also you can start / stop Container with Container ID : docker start/stop {Container ID}
	    


If you want to run test with console

   - Go to 'StorytelTest' directory with command line.
    
   - Execute : dotnet test
    


## Notes:

   1) This application's documentation used by Swagger. You can run API and automatic redirect to Swager Page.
    
       - Swagger URL is : http://localhost:{port}/swagger/index.html
	
   2) Code challenge asks about Message entity. Because of this my concern in Message entity but i develop user part for authentication and authorization.
    
   3) Because of simplicity, I used JWT token and every time you want to call API, you can add token as Bearer token.
    
   4) Because i assume that the client is in another place or server. I enable Cors Policy.
    
   5) API url under versioning (for now we are using 'v1'. Pay attention to it. for more informations, you can use swagger page.
    
   6) Because our focus on Message Entity, So only write test for that.
    
