var app = new Vue({
    el: '#app',
    data: {
        message: 'Hello Vue!',
        apiKey: '',
        apiSecret: '',
        projects: [],
        signedIn: false,
        warningMessage: '',
        languageKeyImport: '',
        deleteMissingTranslations: true
    },
    methods: {
        loadData: function () {
            var self = this;
            // Make a request for a user with a given ID
            axios.get('/api/v1/Display')
                .then(function (response) {
                    self.projects = response.data;
                    self.warningMessage = '';
                })
                .catch(function (error) {
                    console.log(error);
                    self.warningMessage = error.message;
                });
        },
        signIn: function (event) {
            if (event) {
                event.preventDefault();
            }

            axios.defaults.headers.common['ApiKey'] = this.apiKey;
            axios.defaults.headers.common['ApiSecret'] = this.apiSecret;

            this.signedIn = true;
            localStorage.setItem('apiKey', this.apiKey);
            localStorage.setItem('apiSecret', this.apiSecret);
            this.loadData();
        },
        signOut: function (event) {
            if (event) {
                event.preventDefault();
            }

            this.signedIn = false;
            this.apiKey = '';
            this.apiSecret = '';
            this.projects = [];
            localStorage.removeItem('apiKey');
            localStorage.removeItem('apiSecret');
        },
        downloadFile: function (project, event) {
            if (event) {
                event.preventDefault()
            }
            var self = this;
            axios({
                url: '/api/v1/Export/' + project.language + '/' + project.key,
                method: 'GET',
                responseType: 'blob',
            }).then((response) => {
                self.warningMessage = '';
                const url = window.URL.createObjectURL(new Blob([response.data]));
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', project.key + '_' + project.language + '.csv');
                document.body.appendChild(link);
                link.click();
                }).catch(function (error) {
                    console.log(error);
                    self.warningMessage = error.message;
            });
        },
        importFile: function (event) {
            if (event) {
                event.preventDefault()
            }
            var self = this;
            var formData = new FormData();
            var importFile = document.querySelector('#importFileInput');
            formData.append("importFile", importFile.files[0]);
            formData.append('languageKeyImport', self.languageKeyImport);
            formData.append('deleteMissingTranslations', self.deleteMissingTranslations);
            axios.post('/api/v1/Import', formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            }).then(function (response) {
                self.warningMessage = 'data saved';
                }).catch(function (error) {
                    console.log(error);
                    self.warningMessage = error.message + ' ' + error.response.data.message;
            });
        }
    },
    mounted: function () {
        var key = localStorage.getItem('apiKey');
        var secret = localStorage.getItem('apiSecret');

        if (key && secret) {
            this.apiKey = key;
            this.apiSecret = secret;
            this.isSignedIn = true;
            this.signIn();
        }
    },
});