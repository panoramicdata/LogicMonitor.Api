import-module LogicMonitor.Api

<#
	My Function
#>
function Get-Function {

}

<#
	Gets a PortalClient
#>
function Get-PortalClient {
	return new-object LogicMonitor.Api::PortalClient
}