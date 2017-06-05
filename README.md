# Code Sample: Banking Ledger

### Colin Lebens

## Summary
Provides a very simple banking service that provides a service layer that is deployable with both Console and Web interfaces. 

The following functionalities are supported:
- Account Creation and Login
- Recording Deposits
- Recording Withdrawals
- Displaying Account Balance
- Viewing Transaction History

## Usage

### To Load
Open ledgerdemo\ledgerdemo.sln in Visual Studio

### For Console Version 
- Right click ledgerdemo.ConsoleApp in Solution Explorer, selet 'Set as StartUp Project', and start Debugger
- Create account with 'Create Account' menu option 
- Log in to created account with 'Manage Account' menu option
- Use Account Manager menu to perform all account operations
- Logout by exiting the Account Manager menu

### For Web Version 
- Right click ledgerdemo.Web in Solutoin Explorer, selet 'Set as StartUp Project', and start Debugger
- Create account on the account creation page, accessed by clicking Create Account button
- Should log created user in automatically, but future logins achieved with login dialog in Nav bar
- Perform all operations on Account page (accessed at '/Account')
- Logout by clicking Logout button