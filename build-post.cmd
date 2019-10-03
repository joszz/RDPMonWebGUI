@echo off

if $(ConfigurationName) == Debug (
	C:\Windows\System32\inetsrv\appcmd.exe start apppool /apppool.name:"RDPMon"
)

exit 0