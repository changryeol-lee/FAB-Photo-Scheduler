import axios from 'axios'

// 환경 변수에서 API Gateway URL과 API Key 가져오기
const apiUrl = process.env.API_GATEWAY_URL
const apiKey = process.env.API_KEY
const defaultDbName = 'FACTORY_FAB'

const engineApiUrl = 'http://localhost:7043'

const api = axios.create({
  baseURL: apiUrl, // API Gateway 기본 URL 설정
  headers: {
    'x-api-key': apiKey, // 모든 요청에 API Key 포함
    'Content-Type': 'application/json',
  },
})

api.interceptors.request.use((config) => {
  if (!defaultDbName) return config

  if (config.method === 'post') {
    if (typeof config.data === 'object' && config.data !== null) {
      config.data = { dbName: defaultDbName, ...config.data }
    } else {
      config.data = { dbName: defaultDbName }
    }
  }

  if (config.method === 'get') {
    config.params = {
      dbName: defaultDbName,
      ...(config.params || {}),
    }
  }

  return config
})

// 엔진 서버용 Axios 인스턴스
const engineApi = axios.create({
  baseURL: 'http://52.79.235.205:7043', // 엔진 API URL 설정
  headers: {
    'Content-Type': 'application/json',
  },
})

export { api, engineApi }
export default api
