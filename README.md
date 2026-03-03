README including:
## Setup steps
    ### Frontend Setup

        Make sure you have **Node.js** installed on your machine.

        After cloning the repository:

        1. Navigate to the frontend project folder.
        2. Install dependencies:
        ```
        npm install
        ```
        The project uses:
        * **Tailwind CSS** (for styling)
        npm install -D tailwindcss@3 postcss autoprefixer

        * **Axios** (for API calls)
        npm install axios
        3. Start the frontend:
        ```
        npm start
        ```
        ###  Backend Setup

        Make sure you have **.NET SDK** installed (In my case API is made on .net 8).

        Steps:

        1. Navigate to the backend project folder.
        2. Restore dependencies:

        ```
        dotnet restore
        ```

        The backend uses:

        * **ASP.NET Core Web API**
        * **SQLite**
        * **Microsoft.Data.Sqlite** library
        ```
        dotnet add package Microsoft.Data.Sqlite (library installation command)
        ```
        3. Run the backend:
        The SQLite database file (`user.db`) will be created automatically when the application runs.

## Architecture explanation
    ### Backend explanation
                The project follows a simple layered structure:

            1. Controllers Layer
                    TransactionController
                    DashboardController

            Responsible for:
                Handling HTTP requests
                Returning appropriate HTTP responses
                Basic exception handling

            2. Service Layer
                    TransactionService
                    DashboardService

            Responsible for:
                    Business logic
                    Fraud rule evaluation
                    Database interaction

            This keeps logic separate from the API layer.

            3. DTO Layer
                    TransactionDto
                    TransactionDataDto
                    DashBoardDto

            DTOs define request and response models and include validation attributes like [Required].

            4. Database
            SQLite is used as a relational database.
            Data is stored in a transactions table.
            Duplicate transaction_id is prevented using a unique constraint.
            Indexed field: user_id (for faster lookup during rule evaluation).
            The architecture is kept simple to match the assignment scope.

    ### Frontend Architecture
                App.js
                    Acts as the main container. It manages shared state (such as refresh and user filter) and renders all components.

                TransactionsTable.js
                    Fetches and displays transaction data from the API.
                    It also highlights flagged transactions.

                TransactionForm.js
                    Provides a form to submit new transactions to the backend API.

                Dashboard.js
                    Displays summary statistics such as total transactions, flagged transactions, high-risk count, and suspicious count.

                Api.js (Service Layer)
                    Contains all API call logic using Axios.
                    This keeps API communication separate from UI components for better structure and maintainability.  
                

## How rule logic is structured
   Fraud rule logic is implemented inside TransactionService.

    When a transaction is received:
            A database connection is opened.
            The system checks how many transactions the user made in the last 1 minute.
            Rules are evaluated before inserting the transaction.
            Implemented Rules
                Rule 1 – High Risk
                    If amount > 20,000
                    Mark high_risk = true
                Rule 2 – Suspicious
                    If a user has made more than 3 transactions within the last 1 minute
                    Mark suspicious = true
            The rule evaluation happens inside:
                PerformTransaction()

            Flags are stored directly in the database fields:
                high_risk
                suspicious

            Dashboard statistics are calculated using SQL aggregation querie
## Assumptions
        While building this system, the following assumptions were made:
        Amount must be positive (negative transactions are not allowed).
        Timestamp is optional; if not provided, UTC Now is used.
        Fraud rules are evaluated only at transaction insertion time.
        Past transactions are not reprocessed.
        Rule 2 uses a 1-minute window instead of lifetime transaction count to better simulate real-world suspicious behavior beacuse if rule 2 is to be applied as is then in the real world scenerio all the users would become suspecious. 
        No authentication is required as this was not made part of the requirement.
        Index is made against the user_id becasue this make data lookup faster and also was the part of the requirement
        SQLite is sufficient for assignment scope.

## How it could scale to 1M transactions/day
To handle around 1 million transactions per day, the system can be improved in the following ways:

    Split the transactions table based on time (e.g., monthly partitions).
    This ensures the database scans only relevant time ranges and keeps performance stable as data grows.
    An index on user_id improves:

    Faster user-based filtering.
    Faster fraud rule checks for user activity.
    Avoidance of full table scans.
    If transaction volume increases further, a composite index on (user_id, timestamp) can be added to optimize time-based fraud checks even more.
    Return transactions in smaller pages (e.g., 50 records per request) instead of sending all records at once.
    This improves API response time and frontend performance.
    Select only the necessary columns and apply filters before returning results.
    This reduces database load and improves efficiency.


Use of AI Tools
 ChatGPT was used to to improve the docuementation. All the business logic and the Database design is made by me independently.
 Syntax error and logical error that occured during the process was resolved using AI. For Instance while returning the Object I was getting error in the Dto I resolved that error using GPT similarly return type error etc
 AI assisted me in generating some pieces of syntax like for instance AddWithValue,ExecuteTransactionQuery,dateTime.UtcNow. 
 The queries was corrected using AI 
 The Frontend design was made by AI rest the logical implementation is done by myself independently