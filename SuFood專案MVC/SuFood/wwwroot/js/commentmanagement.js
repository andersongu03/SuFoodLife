let app = new Vue({
    el: ".CommentReview",
    data: {
        deleteId: undefined,
        topic: '評論管理',
        kesi: "",
        or: [],
        modalContentStyle: {
            w200: true,
            w800: false
        },
        modalContainerStyle: {
            showModal: false,
        },
        isActive: false,
    },
    request: {
        ordersId: 0,
        comment: "",
    },
    //computed: {
    //    filterComment() {
    //        arr = this.OrdersReview.filter(c => {
    //            return c.ReviewId
    //        })
    //    }
    //},
    methods: {
        //DeleteComment:function(c){
        //    console.log('觸發')
        //    this.deleteId = c.ordersId,
        //    this.openModal()
        //},
        openModal() {
            this.modalContainerStyle.showModal = true;
        },
        closeModal() {
            this.modalContainerStyle.showModal = false;
        },
        GetComments() {
            axios.get("/BackStage/CommentManagement/GetComment").then(response => {
                this.or = response.data
            })

        },
        DeleteComment: function (id) {
            /*console.log("觸發");*/
            var _this = this
            axios.delete(`/BackStage/CommentManagement/DeleteComment/${id}`).then(response => {
                this.kesi = response.data;
                _this.GetComments();
            })
        },


    },
    mounted() {
        this.GetComments();
    }
})