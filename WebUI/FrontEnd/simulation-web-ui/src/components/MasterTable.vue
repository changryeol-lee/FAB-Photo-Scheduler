<template>
  <q-card class="table-card">
    <q-card-section class="q-pb-none">
      <div class="row items-center justify-between">
        <div class="text-h6">{{ title }}</div>
        <q-btn icon="refresh" flat round dense :loading="loading" @click="$emit('refresh')" />
      </div>
    </q-card-section>

    <q-card-section>
      <q-table
        :style="{ height: fixTableHeightPx + 'px' }"
        :rows="displayRows"
        :columns="columns"
        :loading="loading"
        row-key="id"
        dense
        selection="single"
        sticky-header
        virtual-scroll
        :virtual-scroll-slice-size="6"
        :pagination="pagination"
      >
        <template v-slot:loading>
          <q-inner-loading showing color="primary" />
        </template>
        <template v-slot:no-data>
          <div class="full-width row flex-center q-pa-md text-grey-8">데이터가 없습니다</div>
        </template>
      </q-table>
    </q-card-section>
  </q-card>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import type { QTableColumn } from 'quasar'

const props = defineProps({
  title: {
    type: String,
    required: true,
  },
  data: {
    type: Array,
    required: true,
  },
  columns: {
    type: Array as () => QTableColumn[],
    required: true,
  },
  loading: {
    type: Boolean,
    default: false,
  },
  minRows: {
    type: Number,
    default: 8,
  },
  fixTableHeightPx: {
    type: Number,
    default: 290,
  },
})
// 이벤트 정의
defineEmits(['refresh'])

// 기본 페이지네이션 설정
const pagination = ref({
  rowsPerPage: 0,
})

// 최소 6줄을 보여주기 위한 계산된 속성
const displayRows = computed(() => {
  const dataArray = props.data as any[]
  if (dataArray.length >= props.minRows) {
    return dataArray
  }

  // 부족한 행 수만큼 빈 행 추가
  const emptyRows = Array(props.minRows - dataArray.length)
    .fill(0)
    .map((_, index) => {
      return { _empty: true, _id: `empty-${index}` }
    })

  return [...dataArray, ...emptyRows]
})
</script>

<style scoped>
.table-card {
  height: 100%;
}

.fixed-height-table {
  height: 100%;
}

/* 빈 행은 투명하게 표시 */
:deep(.q-tr[data-empty='true']) {
  opacity: 0;
}

/* 테이블 헤더 스타일 */
:deep(.q-table thead tr) {
  background-color: #f5f5f5;
}

/* 테이블 헤더 고정 CSS */
:deep(.q-table) thead {
  position: sticky;
  top: 0;
  z-index: 1;
}

:deep(.q-table) thead tr th {
  position: sticky;
  top: 0;
  z-index: 1;
  background-color: #f5f5f5;
}

/* 고정 헤더에 그림자 효과 추가 */
:deep(.q-table--sticky-header thead tr:last-child th) {
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}
/* 선택된 행 스타일 */
:deep(.q-table tbody tr.selected) {
  background-color: rgba(25, 118, 210, 0.12);
}

:deep(.q-table--col-auto-width) {
  display: none;
}
:deep(.q-checkbox) {
  display: none;
}

/* 열 사이에 경계선 추가 */
:deep(.q-table th),
:deep(.q-table td) {
  border-right: 1px solid #e0e0e0;
}

/* 마지막 열의 오른쪽 경계선은 제거 (선택 사항) */
:deep(.q-table th:last-child),
:deep(.q-table td:last-child) {
  border-right: none;
}

/* 테두리 스타일을 일관되게 유지 */
:deep(.q-table) {
  border-collapse: collapse;
}
</style>
