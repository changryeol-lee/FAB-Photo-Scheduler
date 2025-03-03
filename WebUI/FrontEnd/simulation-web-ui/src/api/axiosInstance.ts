import axios from 'axios'

// 환경 변수에서 API Gateway URL과 API Key 가져오기
const apiUrl = process.env.API_GATEWAY_URL as string
const apiKey = process.env.API_KEY as string

const api = axios.create({
  baseURL: apiUrl, // API Gateway 기본 URL 설정
  headers: {
    'x-api-key': apiKey, // 모든 요청에 API Key 포함
    'Content-Type': 'application/json',
  },
})

export default api
