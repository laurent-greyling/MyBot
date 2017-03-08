# MyBot

Bots are not a new thing in the world, but lately it has become the best new thing since toast. To create a new bot is fairly simple with [Microsoft's Bot Framework](https://dev.botframework.com/). Here you can find all documentation to start.

For my first bot I will attempt to have a bot do an interview based on the [Nfield system](http://niposoftware.com/), and to keep track of the process I will follow and use documents available in the documentation section of [Microsoft's Bot Framework](https://dev.botframework.com/).

To make it easier I will setout step by step how I followed and build this bot.

##Getting Started

To get started with the bot framework download and install the [Bot Application template](http://aka.ms/bf-bc-vstemplate) and save the zip file to your Visual Studio templates directory which is traditionally in "%USERPROFILE%\Documents\Visual Studio `<year>`\Templates\ProjectTemplates\Visual C#\"

Now you can create a new C# bot application. 

![image](https://cloud.githubusercontent.com/assets/17876815/23705293/69fe7ec8-0409-11e7-8eb4-be8fc0b06116.png)

The default application is a simple echo bot, and will echo back what ever the user type in as communication. For more details see [Getting started documentation](https://docs.botframework.com/en-us/csharp/builder/sdkreference/).

It is always nice to look at some code samples when starting a new project. For those moments you get stuck you can review the samples [here](https://github.com/Microsoft/BotBuilder/tree/master/CSharp).

If you are not an Nfield client and want to write another type of bot, I refer you to the code examples above. The code base here will, however, help you if you want to write a bot that need to reference Html elements and ask questions based on these elements.

##Debugging

Like any other code base you would like to debug and test your code. To test the bot you can use the [bot emulator](https://aka.ms/bf-bc-emulator)

When using the emulator to test your Bot application, make note of the port that the application is running on. You will need this information to run the Bot Framework Emulator.

Now open the Bot Framework Emulator. There are a few items that you will need to configure in the tool before you can interact with your Bot Application.

When working with the emulator with a bot running locally, you need:

 - The Url for your bot set the localhost:<port> pulled from the last step. > Note: will need to add the path "/api/messages" to your URL when using the Bot Application template.
 - Empty out the MicrosoftAppId field
 - Empty out the MicrosoftAppPassword field
 
This will only work with the emulator running locally; in the cloud you would instead have to specify the appropriate URL and authentication values.




Revisit [this](https://docs.botframework.com/en-us/csharp/builder/sdkreference/dialogs.html) later, this is for dialog and not the frame...


##Training links

[Javascript example](https://mva.microsoft.com/en-US/training-courses/getting-started-with-bots-16759?l=2zTAb2HyC_3504668937)

[C# example](https://www.pluralsight.com/courses/microsoft-bot-framework-getting-started) - need subscription
