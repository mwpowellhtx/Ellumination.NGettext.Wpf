How to Run the Tests
====================

Make sure the nuget packages of the solution are restored and then invoke

    > pester
    
from the Package Manager Console.

If you try the more conventional [`Invoke-Pester`](https://pester.dev/docs/commands/invoke-pester) you might get all kinds of errors, because your local Pester installation might not be compatible with the _Pester tests_ in this repository. See also [_source_](https://github.com/pester/Pester).

May also consider [_PowerShell Tools for Visual Studio 2022_](https://marketplace.visualstudio.com/items?itemName=AdamRDriscoll.PowerShellToolsVS2022), which we think includes _Pester Test adapter_ as well.
