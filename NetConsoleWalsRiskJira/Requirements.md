# Requirements

- We are creating a risk evaluation application.
- This application will assess the risk level and effort based on the fields of a Jira ticket.
- It will take a n-Dimensional array of doubles as input.
- The n-Dimensional array will be based on text embedding as well as other parameters derived from the.
- This will be a machine learning application.
- This application will be built using .NET 9.
- Create a console application that can train a weighted average least squares regression model.
- The application will also run the model outputting the risk estimation and the effort estimation.
- Use any .Net Machine Learning Library to generate the models and run the model.
- Generate Unit Tests with NUnit to test model generation and evaluation.
- Some of the dimensions of the input array will include text embeddings from the Jira ticket description and title.
  - These elements will be merged along with other fields from the Jira ticket.
  - This application will need to convert these field values to doubles to be included in the input array.
- When using the console application, we will inject a JSON representation of the Jira ticket into the application.
  - This will be converted into the n-Dimensional array of doubles.
  - The application will then use this array as input to the trained model for risk and effort estimation.

