const path = require('path');
const colors = require('colors/safe');
const fs = require('fs');
var git = require('git-rev-sync');

const appVersion = require('../package.json').version;
console.log(colors.cyan('\nRunning pre-build tasks'));
const versionFilePath = path.join(__dirname + '/../src/environments/version.ts');

const src = `
export const appVersion = '${appVersion}';\r\n
export const gitShort = '${git.short()}';\r\n
export const gitLong = '${git.long()}';\r\n
export const gitDate = '${git.date()}';\r\n
export const gitBranch = '${git.branch()}';
`;

// ensure version module pulls value from package.json
fs.writeFile(versionFilePath, src, { flat: 'w' }, function (err) {
    if (err) {
        return console.log(colors.red(err));
    }

    console.log(colors.green(`Updating application version ${colors.yellow(appVersion)}`));
    console.log(`${colors.green('Writing version module to ')}${colors.yellow(versionFilePath)}\n`);
});

console.log(`${colors.green('Copy builder files')}`);
fs.createReadStream('builder/common.js').pipe(fs.createWriteStream('node_modules/@angular/cli/models/webpack-configs/common.js'));