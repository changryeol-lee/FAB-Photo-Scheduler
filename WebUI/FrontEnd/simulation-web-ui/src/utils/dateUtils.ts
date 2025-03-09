/**
 * 날짜 관련 유틸리티 함수 모음
 */
import { date } from 'quasar'

export const removeZAndParse = (dateString: string): Date => {
  return new Date(dateString.replace('Z', ''))
}

export const formatDate = (dateStr: any) => {
  return date.formatDate(dateStr, 'YYYY-MM-DD')
}
export const formatDateTime = (dateObj: any) => {
  return date.formatDate(dateObj, 'YYYY-MM-DD HH:mm:ss')
}

export const addDays = (dateString: string, days: number): string => {
  const date = new Date(dateString)
  date.setDate(date.getDate() + days)
  return `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, '0')}-${String(date.getDate()).padStart(2, '0')}`
}
