import axios from 'axios';

axios.defaults.baseURL = '/api';

const apiHandler = {
    get: (url, data, responseType) => {
        return new Promise((resolve, reject) => {
            axios({
                method: 'get',
                url,
                data,
                headers: {
                    Accept: 'application/json',
                    'Content-Type': 'application/json; charset=utf-8',
                    withCredentials: true,
                },
                responseType: (responseType == null || responseType == '') ? 'json' : responseType
            }).then(response => {
                if (response.status == 200) {
                    resolve(response);
                } else {
                    reject(response);
                }
            })
        })
    },
    post: (url, data, responseType) => {
        return new Promise((resolve, reject) => {
            axios({
                method: 'post',
                url,
                data,
                headers: {
                    Accept: 'application/json',
                    'Content-Type': 'application/json; charset=utf-8',
                    withCredentials: true,
                },
                //默认json格式，如果是下载文件，需要传 responseType:'blob'
                responseType: (responseType == null || responseType == '') ? 'json' : responseType
            }).then(response => {
                if (response.status == 200) {
                    //根据实际情况进行更改
                    resolve(response)
                } else {
                    reject(response)
                }
            })
        })
    }
}