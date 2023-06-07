require('dotenv').config()

const { Client, GatewayIntentBits } = require('discord.js')
const discordToken = process.env.DISCORD_TOKEN
const prefix = '-'

const client = new Client({ 
    intents: [GatewayIntentBits.MessageContent, GatewayIntentBits.GuildMessages, GatewayIntentBits.Guilds] 
  });
const token = discordToken

client.on('ready', () => {
    console.log(`Bot está online como ${client.user.tag}`);
  });

client.on('message', (message) => {
    console.log('message')
    if (!message.content.startsWith(prefix) || message.author.bot) return;

    const args = message.content.slice(prefix.length).trim().split(/ +/);
    const command = args.shift().toLowerCase();

    if (command === 'ping') {
        message.channel.send('Pong!');
    }
});

client.login(token);