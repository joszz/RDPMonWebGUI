@echo off

if $(ConfigurationName) == Debug (
	C:\Windows\System32\inetsrv\appcmd.exe stop apppool /apppool.name:"RDPMon"
)

exit 0