# BatchToDoCLI
A bulk/batch generator for Microsoft To-Do App tasks

# Setup

## Register a Microsoft Graph app

1. Navigate to the [Azure Active Directory](https://aad.portal.azure.com/) admin center and login with a personal Microsoft Account.

2. Select Azure Active Directory > App Registrations > New Registration.

3. Provide an app name (example: BatchToDoCLI).

4. Under 'Supported account types', set the value 'Personal Microsoft accounts only'.

5. Under Redirect URI, change the dropdown to Public client (mobile & desktop), and set the value to https://login.microsoftonline.com/common/oauth2/nativeclient

6. Click Register.

7. On the next page, copy the 'Application (client) ID and save it.

8. Select 'Authentication' from the 'Manage' menu. 

    Under the 'Mobile and desktop applications' section, add 'http://localhost' to the Redirect URIs list. 

    Under the Avanced settings section, change the 'Allow public client flows' toggle button to 'Yes' and then save the changes.

9. Select 'API permissions' from the 'Manage' menu. Add Calendards.ReadWrite and Tasks.ReadWrite permissions.

