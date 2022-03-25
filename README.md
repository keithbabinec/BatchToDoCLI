# BatchToDoCLI
A bulk/batch generator for Microsoft To-Do App tasks

# Setup

## Register a Microsoft Graph app

In order to authenticate to the Microsoft ToDo API, you need a Microsoft Graph token. Before you can request a Graph token, you must create an app registration with the following steps:

1. Navigate to the [Azure Active Directory](https://aad.portal.azure.com/) admin center and login with a personal Microsoft Account.

2. From the sidebar, select Azure Active Directory > App Registrations > New Registration.

3. Provide an app name (example: BatchToDoCLI).

4. Under 'Supported account types', set the value 'Personal Microsoft accounts only'.

5. Under Redirect URI, change the dropdown to Public client (mobile & desktop), and set the value to https://login.microsoftonline.com/common/oauth2/nativeclient

6. Click Register.

7. On the next page, copy the 'Application (client) ID and save it. You will need this value later for the configuration file.

8. Select 'Authentication' from the 'Manage' menu. 

    Under the Avanced settings section, change the 'Allow public client flows' toggle button to 'Yes' and then save the changes.

9. Select 'API permissions' from the 'Manage' menu. Add Tasks.ReadWrite permissions.

