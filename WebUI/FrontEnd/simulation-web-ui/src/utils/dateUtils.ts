/**
 * 날짜 관련 유틸리티 함수 모음
 */

export const removeZAndParse = (dateString: string): Date => {
  return new Date(dateString.replace('Z', ''))
}
