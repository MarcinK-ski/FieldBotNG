# FieldBotNG
DiscordBot for enabling reverse port forwarding via SSH.

Requirements:
  - Linux or WSL with `net-tools` package
  - SSH auth via private key. __For now, auth by password is not working__
  
Optional:
  - To connect to forwarded port using public address IP, set `GatewayPorts` to `yes` or `clientspecified` in file _sshd\_config_. In case `clientspecified`, bindAddress must be `0.0.0.0` or `\*`.
  

Used libraries:
  - Discord.Net (https://discord.foxbot.me/).