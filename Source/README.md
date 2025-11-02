## Source Code Architecture

### Architecture Diagram

```mermaid
graph TD
    A[Source Code] --> B[Host]
    B --> C[Application]
    C --> D[Repository]
    C --> E[Domain]
    C --> F[Transit]
    D --> E
    D --> F
    A --> G[Webapp]
    G --> F
```

### Host

The Host layer is responsible for managing the application's lifecycle, including initialization, configuration, and
dependency injection. It sets up the environment in which the application runs.

### Application

The Application layer contains the core business logic and orchestrates the interactions between different components.
It is responsible for handling use cases and coordinating data flow between the Repository, Domain, and Transit layers.

### Repository

The Repository layer acts as an intermediary between the Application layer and data sources. It abstracts the data
access logic, providing a clean interface for the Application layer to interact with databases, APIs, or other data
sources.

### Domain

The Domain layer encapsulates the business rules and entities of the application. It defines the core concepts and logic
that are independent of any specific application or infrastructure concerns.

### Transit

The Transit layer handles data transfer between different parts of the application or between the application and
external systems. It is responsible for serialization, deserialization, and communication protocols.

### Webapp

The Webapp layer is responsible for the user interface and user experience of the application. It manages the
presentation logic and interacts with the Application layer to display data and handle user input.

## Webapi Guidelines

### Mind map

```mermaid
graph LR
Webapi(Webapi) --> User["User"]
	User --create--> User.Create["POST /api/user"]
	User --get detail--> User.Detail["GET /api/user/{id}"]
	User --update--> User.Update["PUT /api/user/{id}"]
	User --delete--> User.Delete["DELETE /api/user/{id}"]
	User --get list--> User.Query["GET /api/user"]
	User --get count--> User.Count["GET /api/user/count"]
	User --reset password--> User.ResetPassword["PUT /api/user/{id}/password"]
	User --change password--> User.ChangePassword["PUT /api/user/password"]

Webapi --> Identity["Identity"]
  Identity --grant token--> AuthGrantToken["POST /api/identity/token/grant"]
    AuthGrantToken --- GrantWithUsername["Username"]
    AuthGrantToken --- GrantWithOAuth["OAuth"]
    AuthGrantToken --- GrantWithOTP["One-Time Password"]
  Identity --refresh token--> AuthRefreshToken["POST /api/identity/token/refresh"]
  
Webapi --> Team["Team"]
	Team --get list--> Team.Query["GET /api/team"]
	Team --get count--> Team.Count["GET /api/team/count"]
	Team --detail--> Team.Detail["GET /api/team/{id}"]
	Team --create--> Team.Create["POST /api/team"]
	Team --update--> Team.Update["PUT /api/team/{id}"]
	Team --> Team.Member["Member"]
		Team.Member --member list--> Team.Member.Query["GET /api/team/{id}/member"]
		Team.Member --append member--> Team.Member.Append["POST /api/team/{id}/member"]
		Team.Member --remove member--> Team.Member.Remove["DELETE /api/team/{id}/member [userId...]"]
		Team.Member --quit--> Team.Member.Quit["DELETE /api/team/{id}/quit"]

Webapi --> Project["Project"]
    Project --get list--> Project.Query["GET /api/project"]
    Project --get count--> Project.Count["GET /api/project/count"]
    Project --detail--> Project.Detail["GET /api/project/{id}"]
    Project --create--> Project.Create["POST /api/project"]
    Project --update--> Project.Update["PUT /api/project/{id}"]
    Project --delete--> Project.Delete["DELETE /api/project/{id}"]
    Project --remove image--> Project.RemoveImage["DELETE /api/project/{id}/image"]

Webapi --> Config["Configuration"]

Webapi --> Nodepoint["Nodepoint"]

```
