<template>
  <div class="control-container">
    <div class="control-wrapper q-px-md">
      <div class="control-menu-name">{{ title }}</div>
      <div class="header-container q-pa-md">
        <div class="filter-row">
          <!-- slot으로 전달 -->
          <slot name="filter-items"></slot>
          <div class="search-button-container">
            <q-btn
              :color="buttonColor"
              :icon-right="buttonIcon"
              :label="buttonLabel"
              no-caps
              class="search-button"
              @click="$emit('search')"
            />
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script setup lang="ts">
const props = defineProps({
  // 패널 제목
  title: {
    type: String,
    default: '검색 조건',
  },
  // 검색 버튼 텍스트
  buttonLabel: {
    type: String,
    default: '검색',
  },
  // 검색 버튼 아이콘
  buttonIcon: {
    type: String,
    default: 'search',
  },
  // 검색 버튼 색상
  buttonColor: {
    type: String,
    default: 'primary',
  },
})

// 이벤트 정의
defineEmits(['search'])
</script>
