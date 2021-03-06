
#Dac Runner
Add-Type -path "${Env:ProgramFiles(x86)}\Microsoft SQL Server\110\DAC\bin\Microsoft.SqlServer.Dac.dll"
 
# initialise dacservices with connection string
$ds = New-Object Microsoft.SqlServer.Dac.DacServices "Data Source=#{FinanceLedgerPosting.DatabaseServer};Initial Catalog=#{FinanceLedgerPosting.Database};Integrated Security=True;"
 
# load the publish profile XML to get the deployment options
$dacProfile = [Microsoft.SqlServer.Dac.DacProfile]::Load((Get-Location).Path + "\Content\Mizuho.London.FinanceLedgerPosting.Database.publish.xml")

$options = $dacProfile.DeployOptions

# load the dacpac
$dacpac = (Get-Location).Path + "\Content\Mizuho.London.FinanceLedgerPosting.Database.dacpac"
$dp = [Microsoft.SqlServer.Dac.DacPackage]::Load($dacpac)
 
# deploy the dacpac
$ds.Deploy($dp, '#{FinanceLedgerPosting.Database}', $true, $options)