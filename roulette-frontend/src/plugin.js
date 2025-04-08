export default {
    install(app, options) {
        app.config.globalProperties.$SaveToken = (username, token) => {
            const encodedToken = btoa(`${username}/${token}`);
            sessionStorage.setItem('token', encodedToken);
        }

        app.config.globalProperties.$ReadToken = () => sessionStorage.getItem('token');
        app.config.globalProperties.$ValidateAPIResponse = (statusCode, bypassStatus) => {
            bypassStatus = bypassStatus !== undefined ? bypassStatus : 200;
            if (!(statusCode >=200 && statusCode <300) && statusCode != bypassStatus) {
                return false;
            }
            return true;
        },
        app.config.globalProperties.$ToLowerUsername = (username) =>{
            return username.replace(new RegExp("[A-Za-z]", "g"), (match) => match.toLowerCase());
        }
    }
}