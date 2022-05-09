import axios, { ResponseType } from 'axios'

axios.defaults.baseURL = import.meta.env.VITE_REMOTE_API_ADDRESS
// axios.defaults.baseURL = 'https://localhost:5001'

const BaseApi = {
    get: (url: string, queryData: any, responseType: ResponseType = 'json') => {
        return axios({
            method: 'get',
            url: url,
            params: queryData,
            responseType: responseType,
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json; charset=utf-8',
                withCredentials: true,
            },
        })
    },
    post: (url: string, data: any, responseType: ResponseType = 'json') => {
        return axios({
            method: 'post',
            url: url,
            data: data,
            responseType: responseType,
            headers: {
                Accept: 'application/json',
                'Content-Type': 'application/json; charset=utf-8',
                withCredentials: true,
            },
        })
    },
}

export default BaseApi
